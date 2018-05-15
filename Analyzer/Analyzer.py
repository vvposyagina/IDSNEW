from sklearn import preprocessing
from sklearn.ensemble import ExtraTreesClassifier

def ReadData(path):   
    handle = open(path, 'r')
    dataset = []
    for line in handle:
        temp = line.split(';')           
        last = temp[-1]
        temp.remove(last)  
        dataset.append(temp)     
    
    handle.close()
    return dataset

path = r"E:\Диплом\Прога\Analyzer\test.txt"
dataset = ReadData(path)
sizeO_of_dataset = len(dataset)
target_values = [x % 2 for x in range(sizeO_of_dataset)]
normalized_dataset = preprocessing.normalize(dataset)

model = ExtraTreesClassifier()
model.fit(normalized_dataset, target_values)
print(model.feature_importances_)





