import java.io.File;

public class Program {
    private static final String TRAIN_FOLDER_PATH = "C:\\Users\\User\\Desktop\\Ja\\PeWueR\\sem_6\\Sztuczna inteligencja\\lista4\\wiki_train_34_categories_data";
    private static final String TEST_FOLDER_PATH = "C:\\Users\\User\\Desktop\\Ja\\PeWueR\\sem_6\\Sztuczna inteligencja\\lista4\\wiki_test_34_categories_data";

    public static void main(String[] args) {
        File trainData = ArffComposer.SaveFilesAsArff(TRAIN_FOLDER_PATH, "train");
        File testData = ArffComposer.SaveFilesAsArff(TEST_FOLDER_PATH, "test");
    }
}
