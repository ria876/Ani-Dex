import numpy as np
import PIL 
import tensorflow as tf
import pathlib
import os
import shutil
import random
import math

from tensorflow import keras
from tensorflow.keras import layers
from tensorflow.keras.models import Sequential


def LoadModels(mainModel,mainEncoding,subModels,subEncodings,specialModels,specialEncodings):
    """ Loads the models for identifing the animal images. Also loads the encoding for the outputs and what they link to
        The main model is tells which animal group the image belongs to 
        The sub models tell which animal in the group the image belongs to
        The special models tell which speciesbreed of that animal the species belongs to. 
        Though not all animals have a special model. Some end a sub model.
    """
    # getting the encoding...
    try:
        encodingFile = open("Models\\Encoding\\mainEncode.txt","r")
        for line in encodingFile:
            mainEncoding = {index:label for index, label in line.lower.split(',')}
        encodingFile.close()
    except Exception as e:
        raise Exception("[Error in LoadModels:main model encoding]",e)
    
    try:
        encodingFile = open("Models\\Encoding\\subEncode.txt","r")
        for line in encodingFile:
            group_name, group_encoding = line.split('[')
            group_encoding.removesuffix("]")
            labels = group_encoding.lower.split(', ')
            labels.sort()
            subEncodings[group_name] = {index:label for index, label in labels}
        encodingFile.close()
    except Exception as e:
        raise Exception("[Error in LoadModels:sub models encoding]",e)
    
    try:
        encodingFile = open("Models\\Encoding\\specialEncode.txt","r")
        for line in encodingFile:
            animal_name, special_encoding = line.split('[')
            special_encoding.removesuffix("]")
            labels = special_encoding.lower.split(', ')
            labels.sort()
            specialEncodings[animal_name] = {index:label for index, label in labels}
        encodingFile.close()
    except Exception as e:
        raise Exception("[Error in LoadModels:special models encoding]",e)
    
    
    # getting the models
    try:
        MainModel_folder = pathlib.Path("C:\\Users\\ArtBot\\Desktop\\Uni Stuff\\Year Three\\COMP 3901\\Backend\\Models\\Main Model").with_suffix('')
        MAIN_MODEL_PATH = str(list(MainModel_folder.glob("*.tflite"))[0].absolute())
        interpreter = tf.lite.Interpreter(model_path=MAIN_MODEL_PATH)
        mainModel = interpreter.get_signature_runner('serving_default')
    except Exception as e:
        raise Exception("[Error in LoadModels: main model loading]",e)
    
    try:
        SubModels_folder = pathlib.Path("C:\\Users\\ArtBot\\Desktop\\Uni Stuff\\Year Three\\COMP 3901\\Backend\\Models\\Sub Models").with_suffix('')
        for dir in list(SubModels_folder.glob("*.tflite")):
            SUB_MODEL_PATH = str(dir.absolute())
            interpreter = tf.lite.Interpreter(model_path=SUB_MODEL_PATH)
            subModels[dir.name.split("_")[0]] = interpreter.get_signature_runner('serving_default')
    except Exception as e:
        raise Exception("[Error in LoadModels: sub models loading]",e)
    
    try:
        SpecialModels_folder = pathlib.Path("C:\\Users\\ArtBot\\Desktop\\Uni Stuff\\Year Three\\COMP 3901\\Backend\\Models\\Special Models").with_suffix('')
        for dir in list(SpecialModels_folder.glob("*.tflite")):
            SPECIAL_MODEL_PATH = str(dir.absolute())
            interpreter = tf.lite.Interpreter(model_path=SPECIAL_MODEL_PATH)
            specialModels[dir.name.split("_")[0]] = interpreter.get_signature_runner('serving_default')
    except Exception as e:
        raise Exception("[Error in LoadModels: sub models loading]",e)
    
    return mainModel, mainEncoding, subModels, subEncodings, specialModels, specialEncodings

def MakePrediction(img_arr, mainModel, mainEncoding, subModels, subEncodings, specialModels, specialEncodings):
    """ Takes an image array (img_arr) an identifies what animal the image is depicting. 
        The img_arr is a three dimessional array (3 dimenssions for each colour chanel) that has the values 0 to 255 for representing the 
        intensity or the pixle of that colour chanel
        An 240x240 pixle image will have an image array shape of (240,240,3). i.e: img_arr.shape = (240,240,3). 
    """
    
    main_inputdetails = mainModel.get_input_detials()
    main_inputdict = { input_detail:img_arr for input_detail in main_inputdetails}
    
    main_prediction = tf.nn.softmax(mainModel(**main_inputdict)['outputs'])
    group_label = mainEncoding[np.argmax(main_prediction)]
    
    sub_inputdetails = subModels[group_label].get_input_details()
    sub_inputdict = { input_detail:img_arr for input_detail in sub_inputdetails}
    
    sub_prediction = tf.nn.softmax(subModels[group_label](**sub_inputdict)['outputs'])
    final_prediction = subEncodings[group_label][np.argmax(sub_prediction)]
    
    if final_prediction not in specialEncodings.keys(): return (final_prediction,False,None)
    
    special_inputdetails = specialModels[final_prediction].get_input_details()
    special_inputdict = { input_detail:img_arr for input_detail in special_inputdetails}
    
    special_prediction = tf.nn.softmax(specialModels[final_prediction](**special_inputdict)['outputs'])
    specific_prediction = specialEncodings[final_prediction][special_prediction]
    
    return (final_prediction,True,specific_prediction)    

def MakePredictions(img_arr, mainModel, mainEncoding, subModels, subEncodings, specialModels, specialEncodings):
    """ Same thing as MakePrediction but gives the two top most likly species
    """
    
    main_inputdetails = mainModel.get_input_detials()
    main_inputdict = { input_detail:img_arr for input_detail in main_inputdetails}
    
    main_prediction = tf.nn.softmax(mainModel(**main_inputdict)['outputs'])
    group_label = mainEncoding[np.argmax(main_prediction)]
    
    sub_inputdetails = subModels[group_label].get_input_details()
    sub_inputdict = { input_detail:img_arr for input_detail in sub_inputdetails}
    
    sub_prediction = tf.nn.softmax(subModels[group_label](**sub_inputdict)['outputs'])
    final_prediction = subEncodings[group_label][np.argmax(sub_prediction)]
    
    if final_prediction not in specialEncodings.keys(): return (final_prediction,False,None)
    
    special_inputdetails = specialModels[final_prediction].get_input_details()
    special_inputdict = { input_detail:img_arr for input_detail in special_inputdetails}
    
    specific_predictions = []
    scores = specialModels[final_prediction](**special_inputdict)['outputs']
    for _ in range(2):
        special_prediction = tf.nn.softmax(scores)
        specific_predictions.append(specialEncodings[final_prediction][special_prediction])
        scores.pop(special_prediction)
        
    return (final_prediction,True,specific_predictions)  

def LoadAnimalInfo(animal_type,animal_specific,special_types):
    FILE_PATH = "AnimalInfo\\" + animal_type
    if animal_type in special_types: FILE_PATH = FILE_PATH + "\\" + animal_specific + "\\"+animal_specific+ ".txt"
    else: FILE_PATH = FILE_PATH + "\\" + animal_type +".txt"
    
    info = {}
    animal_file = open(FILE_PATH,"r")
    for line in animal_file:
        # breakpoint()
        if len(line.split(":/:")) != 2: raise Exception(f"[LoadAnimalInfo] file {FILE_PATH} contians an invalid line")
        header, content = line.split(":/:")
        info[header] = content.replace("\n","")
    
    info["Link"] = info["Link"].split("||")
    return info

def LoadAnimalInfo_FromSearchBar(animal_name,special_types):
    specialGroup={
        "bengal":"cat",
        "bombay":"cat",
        "dilute Tortoiseshell":"cat",
        "calico":"cat",
        "ayrshire cattle":"cattle",
        "brown swiss cattle":"cattle",
        "holstein cattle":"cattle",
        "jersey cattle":"cattle",
        "red dane cattle":"cattle",
        "toy terrier":"dog",
        "beagle":"dog",
        "ttalian greyhound":"dog",
        "reddone":"dog",
        "shih tzu":"dog",
        "paplillon":"dog",
        "english fox hound":"dog",
        "capped heron":"bird",
        "egyptian goose":"bird",
        "mandarin duck":"bird",
        "red-Tailed hawk":"bird",
        "scarlet ibis":"bird",
        "takahe":"bird",
    }
    
    found_check = [animal_name.lower() == key.lower() for key in specialGroup.keys()] 
    if not any(found_check) and target not in special_types:return LoadAnimalInfo(animal_name.lower(),"",special_types)
    elif any(found_check): return LoadAnimalInfo(specialGroup[animal_name.lower()],animal_name.lower(),special_types)
    
def PartialyFound(target:str,lst:list[str]):
    founded = []
    for term in lst:
        if target in term: return True
    return False

def SetUpConnection():
    pass


# Variables that are well needed for the exicution of the server. Once it's complete
special_types = ["dog","bird","cat","cattle"]
