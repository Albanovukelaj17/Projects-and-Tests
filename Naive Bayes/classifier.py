import math
import sys, os, argparse, json, pickle



class NaiveBayesDocumentClassifier:
    def __init__(self):

     
        self.model = None

    def train(self, features, labels):
        self.py = {}
        self.pxy = {}

        
       
        documents_count = len(features)
        vocaboulary= set()

        doc_count_per_class = {}
        word_presence_per_class = {}

     
        for label in labels.values():
            if label not in doc_count_per_class:
                doc_count_per_class[label] = 0
                word_presence_per_class[label] = {}

      
        for doc_id, tokens in features.items():
            label = labels[doc_id]
            doc_count_per_class[label] += 1
            for word in set(tokens):
                vocaboulary.add(word)
                if word in word_presence_per_class[label]:
                    word_presence_per_class[label][word] += 1
                else:
                    word_presence_per_class[label][word] = 1

        # P(Y) für jede Klas
        for label, count in doc_count_per_class.items():
            self.py[label] = count / documents_count

        # P(X|Y)
        for label in word_presence_per_class:
            self.pxy[label] = {}
            class_documents_count = doc_count_per_class[label]
            for word, count in word_presence_per_class[label].items():
                self.pxy[label][word] = count / class_documents_count

        
        with open('classifier_model.pickle', 'wb') as f:
            pickle.dump({'Py': self.py, 'Pxy': self.pxy}, f)

    def apply(self, features):
        
        with open('classifier_model.pickle', 'rb') as f:
            model = pickle.load(f)

        py = model['Py']
        pxy = model['Pxy']

        results = {}
        for doc_id, tokens in features.items():
            probabilitys = {}
            for label in py.keys():
                

                probabilitys[label] = math.log(py[label])
                for word in tokens:
                    if word in pxy[label]:
                      

                        probabilitys[label] += math.log(pxy[label][word])
                   


                for word in pxy[label]:
                    if word not in tokens:
                        
                        probabilitys[label] += math.log(1 - pxy[label][word])
            results[doc_id] = max(probabilitys, key=probabilitys.get)
        return results
    """
        applies a classifier to a set of documents. Requires the classifier
        to be trained (i.e., you need to call train() before you can call apply()).

        @type features: dict

        @rtype: dict
        @return: For each document in 'features', apply() returns the estimated class.
                 The return value is a dictionary of the form:
                 {
                   'doc1.html': 'arts',
                   'doc2.html': 'travel',
                   'doc3.html': 'sports',
                   ...
                 }
        """

def read_json(path):
    with open(path) as f:
        data = json.load(f)['docs']
        features, labels = {}, {}
        for f in data:
            features[f] = data[f]['tokens']
            labels[f] = data[f]['label']
        return features, labels

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description='A document classifier.')
    parser.add_argument('--train', help="train the classifier", action='store_true')
    parser.add_argument('--apply', help="apply the classifier (you'll need to train or load a trained model first)", action='store_true')
    args = parser.parse_args()
    classifier = NaiveBayesDocumentClassifier()

    if args.train:
        features, labels = read_json('train.json')
        classifier.train(features, labels)

    if args.apply:
        features, labels = read_json('test.json')
        result = classifier.apply(features)

        count_correct = 0
        total_doc = len(features)
        for doc_id, predicted_label in result.items():
            correct_label = labels[doc_id]
            print(f"Title: {doc_id}\nCorrect Label: {correct_label}\nPredicted Label: {predicted_label}\n")
            if predicted_label == correct_label:
                count_correct += 1
        accuracy = count_correct / total_doc
        
        print(f"Accuracy: {accuracy * 100}%")
      
        print (" ")
      #ohne 0 behandlung , bzw richtige max 86% accuracy bei epsilon = 0.006, 
        #vocaboulary mit alle wörte bringt zu behandlung von log(0) indem train es vermeidet
