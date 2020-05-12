import weka.classifiers.Evaluation;
import weka.classifiers.bayes.NaiveBayes;
import weka.core.Debug;
import weka.core.Instances;
import weka.core.converters.ArffSaver;
import weka.filters.Filter;
import weka.filters.unsupervised.attribute.StringToWordVector;

import java.io.*;

public class Program {
    private static final String ARFF = ".arff";
    private static final String TRAIN_FOLDER_PATH = "C:\\Users\\User\\Desktop\\Ja\\PeWueR\\sem_6\\Sztuczna inteligencja\\lista4\\wiki_train_34_categories_data";
    private static final String TEST_FOLDER_PATH = "C:\\Users\\User\\Desktop\\Ja\\PeWueR\\sem_6\\Sztuczna inteligencja\\lista4\\wiki_test_34_categories_data";
    private static final String TRAINSET = "train";
    private static final String TESTSET = "test";
    private static final String TRAINSET_FILTERED = "train_filtered";
    private static final String TESTSET_FILTERED = "test_filtered";

    public static void main(String[] args) throws Exception {
        Instances trainInstances = GetTrainInstances();
        Log("NaiveBayes");
        NaiveBayes naiveBayes = new NaiveBayes();
        naiveBayes.buildClassifier(trainInstances);

        Instances testInstances = GetTestInstances();
        Evaluation evaluation = new Evaluation(testInstances);
        evaluation.evaluateModel(naiveBayes, testInstances);
        PrintEvaluation(evaluation);



//        Log("Save Instances");
//        boolean saved = SaveInstances(dataset, "train_filtered");
//        System.out.println(saved);
    }

    private static void PrintEvaluation(Evaluation evaluation) {
        Log("Precision:");
        System.out.println(evaluation.precision(0));
        Log("Recall:");
        System.out.println(evaluation.recall(0));
        Log("fMeasure:");
        System.out.println(evaluation.fMeasure(0));
        Log("errorRate:");
        System.out.println(evaluation.errorRate());
    }

    private static Instances GetTestInstances() throws IOException {
        return GetInstances(TESTSET_FILTERED, TESTSET, TEST_FOLDER_PATH);
    }

    private static Instances GetTrainInstances() throws IOException {
        return GetInstances(TRAINSET_FILTERED, TRAINSET, TRAIN_FOLDER_PATH);
    }

    private static Instances GetInstances(String filteredFileName, String basicFileName, String folderPath) throws IOException{
        if(new File(filteredFileName + ARFF).isFile()){
            Log("found filtered instances file");
            return new Instances(new BufferedReader(new FileReader(filteredFileName + ARFF)));
        }
        if (!new File(basicFileName + ARFF).isFile()){
            Log("train arff composer");
            ArffComposer.SaveFilesAsArff(folderPath, basicFileName);
        }
        Log("Read train instances");
        Instances result = new Instances(new BufferedReader(new FileReader(basicFileName + ARFF)));
        Log("train setClassIndex");
        result.setClassIndex(result.numAttributes() - 1);
        Log("train StringWordVectorFilter");
        return StringWordVectorFilter(result);
    }

    private static void SaveInstances(Instances dataset, String filename) throws IOException {
        ArffSaver saver = new ArffSaver();
        saver.setInstances(dataset);
        saver.setFile(new File(filename + ARFF));
        saver.writeBatch();
    }

    private static Instances StringWordVectorFilter(Instances dataset){
        try {
            StringToWordVector stringToWordVector = new StringToWordVector();
            stringToWordVector.setInputFormat(dataset);
            return Filter.useFilter(dataset, stringToWordVector);
        } catch (Exception e) {
            e.printStackTrace();
        }
        return null;
    }

    private static void Log(Object object){
        System.out.println(object);
    }
}
