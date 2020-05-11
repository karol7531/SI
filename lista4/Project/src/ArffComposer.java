import java.io.*;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.ArrayList;
import java.util.TreeSet;
import java.util.List;
import java.util.TreeSet;

public class ArffComposer {
    public static final String RELATION = "WikiClassification";
    public static final String TEXT_ATTRIBUTE = "text";
    public static final String LABEL_ATTRIBUTE = "class";

    public static File SaveFilesAsArff(String directory, String filename){
        ArrayList<String> data =  new ArrayList<>();
        TreeSet<String> labels = new TreeSet<>();
        List<Path> paths = DiectoryReader.ReadDirectory(directory);
        System.out.println("Saving files for " + filename + ".arff");
        int progress = 0;
        for(Path p : paths){
            String label = GetLabel(p);
            labels.add(label);
            String fileContent = GetFileContent(p);
            fileContent = fileContent.replaceAll("\\n|\\r", " ");
            data.add("\"" + fileContent + "\", " + label);
            progress++;
            if(progress % (paths.size() / 10) == 0)
                System.out.println(progress * 10 / (paths.size() / 10) + "%");
        }
        File result = ComposeArff(data, labels, filename);
        System.out.println(result.getAbsolutePath());
        return result;
    }

    private static File ComposeArff(ArrayList<String> data, TreeSet<String> labels, String filename) {
        File result = new File(filename + ".arff");
        try {
            FileOutputStream fos = new FileOutputStream(result);
            BufferedWriter bw = new BufferedWriter(new OutputStreamWriter(fos));

            bw.write("@RELATION " + RELATION);  bw.newLine();
            bw.write("@ATTRIBUTE " + TEXT_ATTRIBUTE + " String");  bw.newLine();
            bw.write("@ATTRIBUTE " + LABEL_ATTRIBUTE + " " + GetLabelsString(labels));  bw.newLine();
            bw.write("@DATA " );  bw.newLine();
            for(String d : data){
                bw.write(d);
                bw.newLine();
            }

            bw.close();
        } catch (FileNotFoundException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
        return result;
    }

    private static String GetLabelsString(TreeSet<String> labels) {
        String result = "{ ";
        for(String l : labels){
            result += l + ", ";
        }
        result = result.substring(0, result.length() - 2);
        return result + " }";
    }

    private static String GetLabel(Path path) {
        return path
                .getFileName()
                .toString()
                .split("_")[0];
    }

    private static String GetFileContent(Path path){
        try {
            return Files.readString(path);
        } catch (IOException e) {
            e.printStackTrace();
        }
        return "";
    }
}
