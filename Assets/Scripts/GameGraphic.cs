using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static GameLogic;
using static GameManager;

public class GameGraphic : MonoBehaviour
{
    public int selectedBottleIndex = -1;

    private GameManager gameManager;

    public BallGraphic prefabBallGraphic;

    public BottleGraphic prefabBottleGraphic;

    private BallGraphic previewBall;

    public List<BottleGraphic> bottleGraphics;

    public Vector3 bottleStartPosition;
    public Vector3 bottleDistance;

    private void Awake()
    {
        gameManager = Object.FindFirstObjectByType<GameManager>();
    }


    private void Start()
    {
        selectedBottleIndex = -1;

        previewBall = Instantiate(prefabBallGraphic);


    }

    public void createBottleGraphic(List<GameManager.Bottle> list)
    {
        // Xóa các chai cũ nếu có
        DestroyAllBottleGraphics();

        // Tạo các chai mới
        foreach (GameManager.Bottle b in list)
        {
            BottleGraphic bg = Instantiate(prefabBottleGraphic);

            // Đặt bg (BottleGraphic) là con của một object khác, ví dụ như `this.transform`
            bg.transform.SetParent(this.transform);

            bottleGraphics.Add(bg);
            List<GameManager.BallType> ballTypes = new List<GameManager.BallType>();

            foreach (var ball in b.balls)
            {
                ballTypes.Add(ball.type);
            }
            bg.SetGraphic(ballTypes.ToArray());
        }

        // Tính toán số hàng và số cột
        int numberOfRows = Mathf.Min(2, Mathf.CeilToInt(bottleGraphics.Count / 4f));
        int numberOfColumns = Mathf.CeilToInt(bottleGraphics.Count / (float)numberOfRows);

        // Tính toán khoảng cách giữa các chai
        Vector3 pos = bottleStartPosition;
        float rowSpacing = bottleDistance.y;
        float columnSpacing = bottleDistance.x;

        for (int i = 0; i < bottleGraphics.Count; i++)
        {
            int row = i / numberOfColumns;
            int column = i % numberOfColumns;

            // Tính toán vị trí dựa trên hàng và cột
            pos.x = bottleStartPosition.x + column * columnSpacing;
            pos.y = bottleStartPosition.y - row * rowSpacing;

            bottleGraphics[i].transform.position = pos;
            bottleGraphics[i].index = i;
        }
    }



    public void AddNewBottleGraphic(GameManager.Bottle newBottle)
    {
        BottleGraphic bg = Instantiate(prefabBottleGraphic);
        bottleGraphics.Add(bg);
        bg.SetGraphic(new GameManager.BallType[0]);

        Vector3 pos = bottleStartPosition + bottleDistance * bottleGraphics.Count;
        bg.transform.position = pos;

        bg.index = bottleGraphics.Count - 1;

    }

    public void DestroyAllBottleGraphics()
    {
        foreach (BottleGraphic bottleGraphic in bottleGraphics)
        {
            Destroy(bottleGraphic.gameObject);
        }
        bottleGraphics.Clear();
    }

    public void RefreshBottleGraphic(List<GameManager.Bottle> bottles)
    {
        for (int i = 0; i < bottles.Count; i++)
        {
            GameManager.Bottle gb = bottles[i];
            BottleGraphic bottleGraphic = bottleGraphics[i];

            List<GameManager.BallType> ballTypes = new List<GameManager.BallType>();

            foreach(var ball in gb.balls)
            {
                ballTypes.Add(ball.type);
            }
            bottleGraphic.SetGraphic(ballTypes.ToArray());
        }
    }

    public void OnClickBottle(int bottleIndex)
    {
        if (isSwitchingBall)
        {
            return;
        }

        Debug.Log($"Selected bottle: {selectedBottleIndex}, Clicked bottle: {bottleIndex}");

        if (selectedBottleIndex == -1)
        {
            if (gameManager.bottles[bottleIndex].balls.Count > 0)
            {
                selectedBottleIndex = bottleIndex;
                Debug.Log($"Selected bottle index set to: {selectedBottleIndex}");
                StartCoroutine(MoveBallUp(bottleIndex));
            }
        }
        else
        {
            if (selectedBottleIndex == bottleIndex)
            {
                StartCoroutine(MoveBallDown(bottleIndex));
                selectedBottleIndex = -1;
            }
            else
            {
                List<SwitchBallCommand> commands = gameManager.CheckSwitchBall(selectedBottleIndex, bottleIndex);
                if (commands.Count > 0)
                {
                    StartCoroutine(SwitchBallCoroutine(selectedBottleIndex, bottleIndex));
                }
                else
                {
                    // Cannot switch, transition selection to new bottle
                    StartCoroutine(TransitionSelectionToNewBottle(selectedBottleIndex, bottleIndex));
                }
            }
        }
    }


    private IEnumerator TransitionSelectionToNewBottle(int oldBottleIndex, int newBottleIndex)
    {
        isSwitchingBall = true;

        // Đưa bóng cũ về vị trí ban đầu
        Vector3 originalPosition = bottleGraphics[oldBottleIndex].GetBallPosition(gameManager.bottles[oldBottleIndex].balls.Count - 1);

        while (Vector3.Distance(previewBall.transform.position, originalPosition) > 0.005f)
        {
            previewBall.transform.position = Vector3.MoveTowards(previewBall.transform.position, originalPosition, 10 * Time.deltaTime);
            yield return null;
        }

        // Đặt lại bóng cũ
        bottleGraphics[oldBottleIndex].SetGraphic(gameManager.bottles[oldBottleIndex].balls.Count - 1, gameManager.bottles[oldBottleIndex].balls[gameManager.bottles[oldBottleIndex].balls.Count - 1].type);

        // Chuyển sang chai mới
        selectedBottleIndex = newBottleIndex;

        // Di chuyển bóng mới lên
        Vector3 newUpPosition = bottleGraphics[newBottleIndex].GetBottleUpPosition();
        List<GameManager.Ball> newBallList = gameManager.bottles[newBottleIndex].balls;

        if (newBallList.Count > 0)
        {
            GameManager.Ball newBall = newBallList[newBallList.Count - 1];
            Vector3 newBallPosition = bottleGraphics[newBottleIndex].GetBallPosition(newBallList.Count - 1);

            bottleGraphics[newBottleIndex].SetGraphicNone(newBallList.Count - 1);

            previewBall.SetColor(BallGraphic.ConvertFromGameType(newBall.type));
            previewBall.transform.position = newBallPosition;
            previewBall.gameObject.SetActive(true);

            while (Vector3.Distance(previewBall.transform.position, newUpPosition) > 0.005f)
            {
                previewBall.transform.position = Vector3.MoveTowards(previewBall.transform.position, newUpPosition, 10 * Time.deltaTime);
                yield return null;
            }
        }

        isSwitchingBall = false;
    }

    private IEnumerator MoveBallUp(int bottleIndex)
    {
        isSwitchingBall = true;

        if (bottleIndex < 0 || bottleIndex >= bottleGraphics.Count)
        {
            Debug.LogWarning($"Invalid bottle index: {bottleIndex}");
            isSwitchingBall = false;
            yield break;
        }

        List<GameManager.Ball> ballList = gameManager.bottles[bottleIndex].balls;

        if (ballList.Count == 0)
        {
            Debug.LogWarning($"No balls in bottle {bottleIndex} to move up.");
            isSwitchingBall = false;
            yield break;
        }

        Vector3 upPosition = bottleGraphics[bottleIndex].GetBottleUpPosition();
        GameManager.Ball b = ballList[ballList.Count - 1];
        Vector3 ballPosition = bottleGraphics[bottleIndex].GetBallPosition(ballList.Count - 1);

        bottleGraphics[bottleIndex].SetGraphicNone(ballList.Count - 1);

        previewBall.SetColor(BallGraphic.ConvertFromGameType(b.type));
        previewBall.transform.position = ballPosition;
        previewBall.gameObject.SetActive(true);

        while (Vector3.Distance(previewBall.transform.position, upPosition) > 0.005f)
        {
            previewBall.transform.position = Vector3.MoveTowards(previewBall.transform.position, upPosition, 10 * Time.deltaTime);
            yield return null;
        }

        isSwitchingBall = false;
    }


    private IEnumerator MoveBallDown(int bottleIndex)
    {
        isSwitchingBall = true;

        if (bottleIndex < 0 || bottleIndex >= bottleGraphics.Count)
        {
            Debug.LogWarning($"Invalid bottle index: {bottleIndex}");
            isSwitchingBall = false;
            yield break;
        }

        List<GameManager.Ball> ballList = gameManager.bottles[bottleIndex].balls;

        if (ballList.Count == 0)
        {
            isSwitchingBall = false;
            yield break;
        }

        Vector3 downPosition = bottleGraphics[bottleIndex].GetBallPosition(ballList.Count - 1);
        Vector3 ballPosition = bottleGraphics[bottleIndex].GetBottleUpPosition();

        previewBall.transform.position = ballPosition;

        while (Vector3.Distance(previewBall.transform.position, downPosition) > 0.005f)
        {
            previewBall.transform.position = Vector3.MoveTowards(previewBall.transform.position, downPosition, 10 * Time.deltaTime);
            yield return null;
        }

        previewBall.gameObject.SetActive(false);

        GameManager.Ball b = ballList[ballList.Count - 1];
        bottleGraphics[bottleIndex].SetGraphic(ballList.Count - 1, b.type);

        isSwitchingBall = false;
    }

    private bool isSwitchingBall = false;

    private IEnumerator SwitchBallCoroutine(int fromBottleIndex, int toBottleIndex)
    {
        isSwitchingBall = true;
        List<GameManager.SwitchBallCommand> commands = gameManager.CheckSwitchBall(fromBottleIndex, toBottleIndex);

        if (commands.Count == 0) 
        {
            Debug.Log("Không thể di chuyển ");

        }
        else
        {
            pendingBalls = commands.Count;

            previewBall.gameObject.SetActive(false);

            for (int i = 0; i < commands.Count; i++) 
            {
                GameManager.SwitchBallCommand command = commands[i];
                Queue<Vector3> moveQueue = GetCommandPath(command);

                if (i == 0)
                {
                    moveQueue.Dequeue();
                }

                StartCoroutine(SwitchBall(command, moveQueue));
                yield return new WaitForSeconds(0.06f);

            }
            //foreach (GameManager.SwitchBallCommand command in commands) 
            //{
            //    StartCoroutine(SwitchBall(command));
            //    yield return new WaitForSeconds(0.1f);
            //}
            while (pendingBalls > 0)
            {
                yield return null;
            }
            gameManager.SwitchBall(fromBottleIndex, toBottleIndex);

            Debug.Log("di chuyen thanh cong");
        }
        selectedBottleIndex = -1;
        isSwitchingBall = false;
    }

    private int pendingBalls = 0;

    private Queue<Vector3> GetCommandPath(GameManager.SwitchBallCommand command)
    {
        Queue<Vector3> queueMovement = new Queue<Vector3>();

        queueMovement.Enqueue(bottleGraphics[command.fromBottleIndex].GetBallPosition(command.fromBallIndex));
        queueMovement.Enqueue(bottleGraphics[command.fromBottleIndex].GetBottleUpPosition());
        queueMovement.Enqueue(bottleGraphics[command.toBottleIndex].GetBottleUpPosition());
        queueMovement.Enqueue(bottleGraphics[command.toBottleIndex].GetBallPosition(command.toBallIndex));

        return queueMovement;
    }
    private IEnumerator SwitchBall(GameManager.SwitchBallCommand command, Queue<Vector3> movment)
    {

        bottleGraphics[command.fromBottleIndex].SetGraphicNone(command.fromBallIndex);

        Vector3 spawnPosition = movment.Peek();

        var ballObject = Instantiate(prefabBallGraphic, spawnPosition, Quaternion.identity);

        ballObject.SetColor(BallGraphic.ConvertFromGameType(command.type));

        //Queue<Vector3> queueMovement = new Queue<Vector3>();

        //queueMovement.Enqueue(bottleGraphics[command.fromBottleIndex].GetBallPosition(command.fromBallIndex));
        //queueMovement.Enqueue(bottleGraphics[command.fromBottleIndex].GetBottleUpPosition());
        //queueMovement.Enqueue(bottleGraphics[command.toBottleIndex].GetBottleUpPosition());
        //queueMovement.Enqueue(bottleGraphics[command.toBottleIndex].GetBallPosition(command.toBallIndex));


        while (movment.Count > 0)
        {
            Vector3 target = movment.Dequeue();

            while (Vector3.Distance(ballObject.transform.position, target) > 0.005f)
            {
                ballObject.transform.position = Vector3.MoveTowards(ballObject.transform.position, target, 10f * Time.deltaTime);
                yield return null;
            }


            yield return null;
        }


        Destroy(ballObject.gameObject);


        bottleGraphics[command.toBottleIndex].SetGraphic(command.toBallIndex, command.type);

        pendingBalls--;
    }


}
