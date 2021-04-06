
#if (UNITY_EDITOR) 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

//functionality for most of the level editor and its inputs
public class EditNotes : MonoBehaviour
{
    public Transform leftSliderPosition;
    public Transform rightSliderPosition;
    public GameObject regularSliderPrefab;
    public GameObject holdSliderPrefab;
    public List<Transform> editorNotes = new List<Transform>();
    public GameObject root;
    public CameraMovement cameraMovement;
    public EditorSliderInterface selectedSlider;
    public RecordManager recordManager;
    public Shader lineShader;
    public Transform linesParent;
    public Color lineColor;


    public void ToggleLines() {
       if(linesParent.gameObject.activeSelf) {
            linesParent.gameObject.SetActive(false);
        } else {
            linesParent.gameObject.SetActive(true);
       }
    }

    //draws a line by instantiating a gameobject and creating a LineRenderer instance for each line
    //not the best performance-wise, so an opengl solution needs to be used if a lot more lines need to be drawn
    void DrawLine(Vector3 start, Vector3 end, float width)
    {
        GameObject myLine = new GameObject();
        myLine.transform.SetParent(linesParent);
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(lineShader);
        lr.startColor = lineColor;
        lr.endColor = lineColor;
        lr.startWidth = width;
        lr.endWidth = width;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
       // GameObject.Destroy(myLine, duration);
    }

    void DrawGrid(int width = 2000, float gridSize = 2.5f) {

        float xInterval;
        Vector3 startPosition;
        Vector3 endPosition;

        for (int i=0; i < width; i++) {
            xInterval = i * gridSize;
            startPosition = new Vector3(xInterval, 20, 0f);
            endPosition = new Vector3(xInterval, -20, 0f);
            DrawLine(startPosition, endPosition, 0.02f);
        }
    }

    private void Start()
    {
        DrawGrid();
    }

    public void MouseButtonDown() {
        if (!root.activeSelf) return;
        int layerMask = 1 << GlobalHelper.editorSliderLayer;


        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100f, layerMask);


        if (hit.collider != null)
        {
            EditorSliderInterface hitInstance = hit.collider.gameObject.GetComponent<EditorSliderInterface>();
            if (hitInstance != selectedSlider)
            {
                if (selectedSlider != null)
                    selectedSlider.Unselect();

                selectedSlider = hitInstance;
                selectedSlider.Select();
            }

        }
        else
        {
            if (selectedSlider != null)
                selectedSlider.Unselect();
            selectedSlider = null;
        }

    }

    public void DeleteKey() {
        if (!root.activeSelf) return;
        if (selectedSlider != null)
        {
            selectedSlider.Destroy();
            selectedSlider = null;
        }
    }

    public void Alpha1() {
        if (!root.activeSelf) return;
        int layerMask = 1 << GlobalHelper.editorStripLayer;

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100f, layerMask);

        if (hit.collider != null)
        {

            if (hit.collider.name == "background_strip_left")
            {
                float spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
                GenerateNote(regularSliderPrefab, leftSliderPosition, spawnPosition, LevelSliderType.LeftSlider);

            }
            else if (hit.collider.name == "background_strip_right")
            {
                float spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
                GenerateNote(regularSliderPrefab, rightSliderPosition, spawnPosition, LevelSliderType.LeftSlider);

            }

        }

    }

    public void Alpha2() {
        if (!root.activeSelf) return;
        int layerMask = 1 << GlobalHelper.editorStripLayer;

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100f, layerMask);

        if (hit.collider != null)
        {
            if (hit.collider.name == "background_strip_left")
            {
                float spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
                GenerateHoldNote(holdSliderPrefab, leftSliderPosition, spawnPosition, 0.6f,
              LevelSliderType.LeftHoldSlider);

            }
            else if (hit.collider.name == "background_strip_right")
            {
                float spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
                GenerateHoldNote(holdSliderPrefab, rightSliderPosition, spawnPosition, 0.6f,
              LevelSliderType.RightHoldSlider);

            }
        }

    }

    public void LeftControlSave() {
        if (!root.activeSelf) return;
        recordManager.QuickSave();
    }

    public void ScrollWheelUp() {
        if (!root.activeSelf) return;
        Camera.main.orthographicSize += 0.1f;

    }

    public void ScrollWheelDown() { 
        if (!root.activeSelf) return;
        Camera.main.orthographicSize -= 0.1f;

    }


    public void ResetLevelData() {
        int children = leftSliderPosition.childCount;
        for (int i = children - 1; i > -1; i--)
        {
            GameObject.Destroy(leftSliderPosition.GetChild(i).gameObject);
        }

        children = rightSliderPosition.childCount;
        for (int i = children - 1; i > -1; i--)
        {
            GameObject.Destroy(rightSliderPosition.GetChild(i).gameObject);
        }

    }

    public void Toggle() {
        if(root.activeSelf) {
            root.SetActive(false);
            cameraMovement.ResetCamera();
        } else {
            root.SetActive(true);
            cameraMovement.StartMoving();
        }
    }

    public void ClearEditorNotes() {
        foreach(Transform editorNote in editorNotes) {
            if(editorNote != null) Destroy(editorNote.gameObject);

        }
        editorNotes.Clear();
    }

    void GenerateNote(GameObject prefab, Transform parent, double xposition, LevelSliderType levelSliderType) {
        Transform editorNoteInstance = Instantiate(prefab).transform;
        EditorSlider editorSlider = editorNoteInstance.GetComponent<EditorSlider>();
        editorSlider.levelSliderType = levelSliderType;
        editorNoteInstance.position = new Vector3((float)xposition, parent.position.y, 25f);
        editorNoteInstance.SetParent(parent);
        editorNotes.Add(editorNoteInstance);
    }

    void GenerateHoldNote(GameObject prefab, Transform parent, double xposition, float length, LevelSliderType levelSliderType)
    {
        Transform editorNoteInstance = Instantiate(prefab).transform;
        EditorHoldSlider editorHoldSlider = editorNoteInstance.GetComponent<EditorHoldSlider>();
        editorHoldSlider.levelSliderType = levelSliderType;
        editorHoldSlider.Initialize(length);
        editorNoteInstance.position = new Vector3((float)xposition, parent.position.y, 25f);
        editorNoteInstance.SetParent(parent);
        editorNotes.Add(editorNoteInstance);
    }

    public void GenerateLeftNote(float spawnx) {
        GenerateNote(regularSliderPrefab, leftSliderPosition, spawnx, LevelSliderType.LeftSlider);
    }
    public void GenerateRightNote(float spawnx)
    {
        GenerateNote(regularSliderPrefab, rightSliderPosition, spawnx, LevelSliderType.RightSlider);
    }

    public void GenerateLeftHoldNote(float spawnx, float length)
    {
        GenerateHoldNote(holdSliderPrefab, leftSliderPosition, spawnx, length,
                 LevelSliderType.LeftHoldSlider);
    }

    public void GenerateRightHoldNote(float spawnx, float length)
    {
        GenerateHoldNote(holdSliderPrefab, rightSliderPosition, spawnx, length,
                 LevelSliderType.RightHoldSlider);
    }



    public void LoadNotes() {
  
        LevelData currentLevel = GlobalHelper.global.currentLevel;
        double speed = currentLevel.sliderSpeed;
        double spawnPosition;
        for (int i = 0; i < currentLevel.hitTimes.Count; i++)
        {
            switch (currentLevel.sliderSpawns[i])
            {
                case LevelSliderType.LeftSlider:

                    spawnPosition = currentLevel.hitTimes[i] * speed;
                    GenerateNote(regularSliderPrefab, leftSliderPosition, spawnPosition, LevelSliderType.LeftSlider);
                    break;
                case LevelSliderType.RightSlider:
                    spawnPosition = currentLevel.hitTimes[i] * speed;
                    GenerateNote(regularSliderPrefab, rightSliderPosition, spawnPosition, LevelSliderType.RightSlider);
                    break;
                case LevelSliderType.LeftHoldSlider:
                    spawnPosition = currentLevel.hitTimes[i] * speed;
                    GenerateHoldNote(holdSliderPrefab, leftSliderPosition, spawnPosition, (float)(currentLevel.holdSliderEndTimes[i] - currentLevel.hitTimes[i]), 
                    LevelSliderType.LeftHoldSlider);
                    break;
                case LevelSliderType.RightHoldSlider:
                    spawnPosition = currentLevel.hitTimes[i] * speed;
                    GenerateHoldNote(holdSliderPrefab, rightSliderPosition, spawnPosition, (float)(currentLevel.holdSliderEndTimes[i] - currentLevel.hitTimes[i]), 
                    LevelSliderType.RightHoldSlider);
                    break;
                default:
                    break;
            }


        }
    }



}
#endif