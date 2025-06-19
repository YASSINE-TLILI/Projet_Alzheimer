# -*- coding: utf-8 -*-
from flask import Flask, request, jsonify
from predictor import AlzheimerPredictor
import os

app = Flask(__name__)


MODEL_PATH = os.path.join(os.path.dirname(__file__), '../models/mymodel.keras')
CLASSES_PATH = os.path.join(os.path.dirname(__file__), '../models/classes.json')

predictor = AlzheimerPredictor(MODEL_PATH, CLASSES_PATH)

@app.route('/predict', methods=['POST'])
def predict_endpoint():
    # Initialisation de la réponse standard
    response = {"status": "success", "prediction": {}}
    
    if 'file' not in request.files:
        response["status"] = "error"
        response["message"] = "Aucun fichier fourni"
        return jsonify(response), 400
        
    file = request.files['file']
    content_type = file.content_type
    file_bytes = file.read()
    
    try:

        raw_result = predictor.predict(file_bytes, content_type)
        
        
        response["prediction"] = {
            "class": raw_result["class"],
            "class_index": raw_result["class_index"],
            "confidence": raw_result["confidence"],
            "probabilities": raw_result["probabilities"]
        }
        
        return jsonify(response)
    
    except Exception as e:
      
        response["status"] = "error"
        response["message"] = str(e)
        return jsonify(response), 500

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5093)