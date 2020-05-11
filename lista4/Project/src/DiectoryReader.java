import java.io.File;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;
import java.util.stream.Stream;

public class DiectoryReader {
    public static List<Path> ReadDirectory(String directory){
        List<Path> paths = new ArrayList<>();
        try (Stream<Path> walk = Files.walk(Paths.get(directory))) {
            paths = walk
                    .filter(Files::isRegularFile)
                    .filter(path -> path.toString().endsWith(".txt"))
                    .collect(Collectors.toList());
        }catch (IOException e) {
            e.printStackTrace();
        }
        return paths;
    }
}
