import weka.classifiers.Evaluation;
import weka.classifiers.bayes.NaiveBayes;
import weka.classifiers.trees.J48;
import weka.core.Instances;
import weka.core.converters.ArffSaver;
import weka.filters.Filter;
import weka.filters.unsupervised.attribute.StringToWordVector;

import java.io.*;
import java.util.Random;

public class Program {
    public static final String ARFF = ".arff";
    private static final String DATA_FOLDER_PATH = "C:\\Users\\User\\Desktop\\Ja\\PeWueR\\sem_6\\Sztuczna inteligencja\\lista4\\data";
    private static final String COMPOSED_DATASET = "composed_dataset";
    private static final String DATASET = "dataset";
    private static final int CROSS_FOLDS_NUM = 10;

    public static void main(String[] args) throws Exception {
        Instances data = GetInstances();
        data.setClassIndex(0);

        Log("Evaluation NaiveBayes");
        Evaluation evaluation = new Evaluation(data);
        evaluation.crossValidateModel(new NaiveBayes(), data, CROSS_FOLDS_NUM, new Random());
        PrintEvaluation(evaluation);

        Log("Evaluation J48");
        evaluation = new Evaluation(data);
        evaluation.crossValidateModel(new J48(), data, CROSS_FOLDS_NUM, new Random());
        PrintEvaluation(evaluation);
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

    private static Instances GetInstances() throws IOException{
        if(new File(DATASET + ARFF).isFile()){
            Log("reading dataset file");
            return new Instances(new BufferedReader(new FileReader(DATASET + ARFF)));
        }
        if (!new File(COMPOSED_DATASET + ARFF).isFile()){
            Log("composing dataset");
            ArffComposer.SaveFilesAsArff(DATA_FOLDER_PATH, COMPOSED_DATASET);
        }
        Log("Reading composed dataset instances");
        Instances result = new Instances(new BufferedReader(new FileReader(COMPOSED_DATASET + ARFF)));
//        Log("setting ClassIndex");
//        result.setClassIndex(result.numAttributes() - 1);
        Log("applying StringWordVectorFilter");
        result = StringWordVectorFilter(result);
        Log("Saving dataset");
        SaveInstances(result, DATASET);
        return result;
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
