import tensorflow as tf
from tensorflow import keras

# Param�tres d'image
IMG_HEIGHT = 128
IMG_WIDTH = 128
BATCH_SIZE = 64

# Charger le mod�le sauvegard�
model = keras.models.load_model('C:/Users/yasst/source/repos/AlzheimerDetector/python-ml-service/models/mymodel.keras')

# Charger les images de test (adapte le chemin selon ton disque)
test_ds = tf.keras.preprocessing.image_dataset_from_directory(
    'C:\\Users\\yasst\\source\\repos\\AlzheimerDetector\\backend-aspnetcore\\Services\\wwwroot\\uploads\\Data-Alzheimer\\test',
    seed=123,
    label_mode='categorical',
    image_size=(IMG_HEIGHT, IMG_WIDTH),
    batch_size=BATCH_SIZE
)

# �valuer le mod�le
result = model.evaluate(test_ds)
print("Test Loss:", result[0])
print("Test Accuracy:", result[1])

# Si tu as utilis� des m�triques suppl�mentaires � l'entra�nement :
if len(result) > 2:
    print("AUC:", result[2])
if len(result) > 3:
    print("Precision:", result[3])
if len(result) > 4:
    print("Recall:", result[4])
