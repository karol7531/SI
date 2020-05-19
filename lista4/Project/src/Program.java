import weka.attributeSelection.InfoGainAttributeEval;
import weka.attributeSelection.Ranker;
import weka.classifiers.Evaluation;
import weka.classifiers.bayes.NaiveBayesMultinomial;
import weka.classifiers.trees.RandomForest;
import weka.core.Instances;
import weka.core.converters.ArffSaver;
import weka.core.stemmers.IteratedLovinsStemmer;
import weka.filters.Filter;
import weka.filters.supervised.attribute.AttributeSelection;
import weka.filters.unsupervised.attribute.StringToWordVector;

import java.io.*;
import java.util.Random;

public class Program {
    public static final String ARFF = ".arff";
//    private static final String DATA_FOLDER_PATH = "C:\\Users\\User\\Desktop\\Ja\\PeWueR\\sem_6\\Sztuczna inteligencja\\lista4\\data";
//    private static final String COMPOSED_DATASET = "composed_dataset";
//    private static final String DATASET = "dataset";
//    private static final String NB_EVAL = "NaiveBayes.eval";
//    private static final String DT_EVAL = "RandomForest.eval";

    private static final String DATA_FOLDER_PATH = "C:\\Users\\User\\Desktop\\Ja\\PeWueR\\sem_6\\Sztuczna inteligencja\\lista4\\data25";
    private static final String COMPOSED_DATASET = "composed_dataset25";
    private static final String DATASET = "dataset25";
    private static final String NB_EVAL = "NaiveBayes25.eval";
    private static final String DT_EVAL = "RandomForest25.eval";
    private static final int CROSS_FOLDS_NUM = 5;
    private static final int WORDS_TO_KEEP = 500;
    private static final double RANKER_THRESHOLD = 0.0;
    private static final boolean EVALUATE = true;
    private static final boolean READ_EVAL = false;

// wybieranie metody ewaluacji z grupy bayes i dt ?
//    parametry RandomForest
//        parametry StringToWordVectorFilter -> badanie wpływu dodania stemera; idf tf; WORDS_TO_KEEP
//          parametry AttributeSelectionFilter -> threshold


    public static void main(String[] args) throws Exception {
        if(EVALUATE){
            Instances data = GetInstances();

            Log("Evaluating NaiveBayesMultinomial");
            Evaluation NBEval = new Evaluation(data);
            NBEval.crossValidateModel(new NaiveBayesMultinomial(), data, CROSS_FOLDS_NUM, new Random(1));
            weka.core.SerializationHelper.write(NB_EVAL, NBEval);
            PrintEvaluation(NBEval);

            Log("Evaluating RandomForest");
            Evaluation DTEval = new Evaluation(data);
            DTEval.crossValidateModel(new RandomForest(), data, CROSS_FOLDS_NUM, new Random(1));
            weka.core.SerializationHelper.write(DT_EVAL, DTEval);
            PrintEvaluation(DTEval);
        }
        else{
            if(new File(NB_EVAL).isFile()){
                Log("Reading NaiveBayesMultinomial evaluation");
                Evaluation eval = (Evaluation)weka.core.SerializationHelper.read(NB_EVAL);
                PrintEvaluation(eval);
            }
            if(new File(DT_EVAL).isFile()){
                Log("Reading RandomForest evaluation");
                Evaluation eval = (Evaluation)weka.core.SerializationHelper.read(DT_EVAL);
                PrintEvaluation(eval);
            }
        }
    }

    private static Instances GetInstances() throws Exception {
        if(READ_EVAL && new File(DATASET + ARFF).isFile()){
            Log("reading dataset file");
            Instances result = new Instances(new BufferedReader(new FileReader(DATASET + ARFF)));
            Log("setting ClassIndex");
//            result.setClassIndex(0);
            result.setClassIndex(result.numAttributes() - 1);
            return result;
        }
        if (!new File(COMPOSED_DATASET + ARFF).isFile()){
            Log("composing dataset");
            ArffComposer.SaveFilesAsArff(DATA_FOLDER_PATH, COMPOSED_DATASET);
        }
        Log("Reading composed dataset instances");
        Instances result = new Instances(new BufferedReader(new FileReader(COMPOSED_DATASET + ARFF)));
        Log("setting ClassIndex");
        result.setClassIndex(result.numAttributes() - 1);
        Log("applying StringWordVectorFilter");
        result = StringToWordVectorFilter(result);
        Log("applying attribute selection");
        result = AttributeSelectionFilter(result);
        Log("Saving dataset");
        SaveInstances(result, DATASET);
        return result;
    }

    private static void PrintEvaluation(Evaluation evaluation) throws Exception {
        System.out.println(evaluation.toSummaryString());
//        System.out.println(evaluation.toClassDetailsString());

        //  System.out.println(eval.toCumulativeMarginDistributionString());
        //  System.out.println(eval.toMatrixString());
    }

    private static void SaveInstances(Instances dataset, String filename) throws IOException {
        ArffSaver saver = new ArffSaver();
        saver.setInstances(dataset);
        saver.setFile(new File(filename + ARFF));
        saver.writeBatch();
    }

    private static Instances StringToWordVectorFilter(Instances dataset){
        try {
            StringToWordVector stringToWordVector = new StringToWordVector();
            stringToWordVector.setInputFormat(dataset);

            stringToWordVector.setWordsToKeep(WORDS_TO_KEEP);
            stringToWordVector.setOutputWordCounts(true);
            stringToWordVector.setIDFTransform(true);
            stringToWordVector.setTFTransform(true);

//            stringToWordVector.setMinTermFreq(5);
            stringToWordVector.setStemmer(new IteratedLovinsStemmer());
            return Filter.useFilter(dataset, stringToWordVector);
        } catch (Exception e) {
            e.printStackTrace();
        }
        return null;
    }

    private static Instances AttributeSelectionFilter(Instances dataset) throws Exception {
        AttributeSelection filter = new AttributeSelection();
        InfoGainAttributeEval eval = new InfoGainAttributeEval();
        Ranker search = new Ranker();
        search.setThreshold(RANKER_THRESHOLD);
        filter.setEvaluator(eval);
        filter.setSearch(search);
        filter.setInputFormat(dataset);
        return Filter.useFilter(dataset, filter);
    }

    private static void Log(Object object){
        System.out.println(object);
    }
}
