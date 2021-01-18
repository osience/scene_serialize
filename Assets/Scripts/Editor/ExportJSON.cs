using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using LitJson;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class MapObjectsExporter : Editor
{
    [MenuItem("Tools/ExportBuildings")]
    static void Buildings2Json()
    {
        Scene curScene = EditorSceneManager.GetActiveScene();

        string filePath = Path.Combine(Application.streamingAssetsPath, curScene.name + "_buildings.txt");
        FileInfo fileInfo = new FileInfo(filePath);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        StreamWriter streamWriter = fileInfo.CreateText();

        StringBuilder strContent = new StringBuilder();
        JsonWriter writer = new JsonWriter(strContent);

        writer.WriteObjectStart();
            writer.WritePropertyName("scene");
            writer.Write(curScene.name);
            writer.WritePropertyName("gameObjects");
            writer.WriteObjectStart();
                var buildings = Object.FindObjectsOfType(typeof(BuildingInfo));
                Transform transform;
                foreach (BuildingInfo building in buildings)
                {
                    transform  = building.transform;

                    writer.WritePropertyName(building.id.ToString());// 唯一id应该由后端生成 ？？？
                    writer.WriteObjectStart();
                        writer.WritePropertyName("name");
                        writer.Write(building.name);

                        writer.WritePropertyName("position");
                        writer.WriteObjectStart();
                        writer.WritePropertyName("x");
                            writer.Write(transform.position.x.ToString("F5"));
                            writer.WritePropertyName("y");
                            writer.Write(transform.position.y.ToString("F5"));
                            writer.WritePropertyName("z");
                            writer.Write(transform.position.z.ToString("F5"));
                        writer.WriteObjectEnd();

                        writer.WritePropertyName("rotation");
                        writer.WriteObjectStart();
                            writer.WritePropertyName("x");
                            writer.Write(transform.rotation.eulerAngles.x.ToString("F5"));
                            writer.WritePropertyName("y");
                            writer.Write(transform.rotation.eulerAngles.y.ToString("F5"));
                            writer.WritePropertyName("z");
                            writer.Write(transform.rotation.eulerAngles.z.ToString("F5"));
                        writer.WriteObjectEnd();

                        writer.WritePropertyName("prefabName");
                        writer.Write(building.config.name);
                    writer.WriteObjectEnd();
                }
            writer.WriteObjectEnd();
        writer.WriteObjectEnd();

        streamWriter.WriteLine(strContent.ToString());
        streamWriter.Close();
        streamWriter.Dispose();
        AssetDatabase.Refresh();
    }
}