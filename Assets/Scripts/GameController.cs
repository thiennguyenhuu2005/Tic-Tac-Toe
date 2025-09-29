using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameControler : MonoBehaviour
{
    public int whoTurn; // 0 = x, 1 = o
    public int turnCount; //counts the number of turn played
    public GameObject[] turnIcons; // display whos turn it is 
    public Sprite[] playIcons ; // 0 = x icon, 1 = o icon
    public Button[] tictactoeSpaces; // playable space for our game
    public int[] markedspaces; // ID's which space was marked by which player
    public TMP_Text[] winnerText; // hold the text component of the winner text
    public GameObject[] winningLine; // hold all the different line for show that ther is a winner
    public GameObject winnerPanel;
    public int xPlayerScore;
    public int oPlayerScore;
    public TMP_Text xPlayerScoreText;
    public TMP_Text oPlayerScoreText;
    public Button xPlayersButton;
    public Button oPlayresButton;
    public bool playWithAI; //fasle = play normal , true = play with AI
    public GameObject modePanel;
    // Start is called before the first frame update
    
    void Start()
    {
        GameSetup();
    }
    public void playNormal(){
        playWithAI = false; 
        modePanel.SetActive(false);
    }
    public void PlayWithAI(){
        playWithAI = true;
        modePanel.SetActive(false);
    }
    void GameSetup(){
        whoTurn = 0;
        turnCount = 0;
        turnIcons[0].SetActive(true);
        turnIcons[1].SetActive(false);
        for(int i = 0;i<tictactoeSpaces.Length;i++){
            tictactoeSpaces[i].interactable = true;
            tictactoeSpaces[i].GetComponent<Image>().sprite = null;
        }
        for(int i = 0; i<tictactoeSpaces.Length;i++){
            markedspaces[i] = -100; 
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void TicTacToeButton(int whichNumber){
        xPlayersButton.interactable = false;
        oPlayresButton.interactable = false;

        tictactoeSpaces[whichNumber].image.sprite = playIcons[whoTurn];
        tictactoeSpaces[whichNumber].interactable = false;
        markedspaces[whichNumber] = whoTurn + 1;
        turnCount ++;

        if (turnCount > 4){
            bool flag = WinnerCheck();
            if(flag){
                return;
            }
        }
        if (whoTurn == 0) {
            whoTurn = 1;
            turnIcons[0].SetActive(false);
            turnIcons[1].SetActive(true);
            if(playWithAI){
                AIMove();
            }
        }
        else {
            whoTurn = 0;
            turnIcons[0].SetActive(true);
            turnIcons[1].SetActive(false);
        }
    }

    void AIMove ()
    {
        int moveIndex = FindBestMove(1,0);

        tictactoeSpaces[moveIndex].image.sprite = playIcons[whoTurn];
        tictactoeSpaces[moveIndex].interactable = false;
        markedspaces[moveIndex] = whoTurn + 1;
        turnCount++;

        if (turnCount > 4) {
            WinnerCheck();
        }

        whoTurn = 0;
        turnIcons[0].SetActive(true);
        turnIcons[1].SetActive(false);
    }
    int FindBestMove (int AITurn,int playerTurn) // AI turn = 1
    {
        
        if (turnCount == 0 && whoTurn == AITurn){
            return 4;
        }else if(turnCount >= 3){
            int indexAttack = indexNextWin(AITurn);
            if(indexAttack != -100){
                return indexAttack;
            }

            int indexDefend = indexNextWin(playerTurn);
            if(indexDefend != -100){
                return indexDefend;
            }
        }
        List<int> emptyCells = new List<int>();
        for (int i = 0; i < tictactoeSpaces.Length; i++) {
            if (markedspaces[i] == -100) {
                emptyCells.Add(i);
            }
        }
        if (emptyCells.Count > 0) {
            int randomIndex = Random.Range(0, emptyCells.Count);
            return emptyCells[randomIndex];
        }
        return -1;
    }

    int indexNextWin(int whichTurn)
    {
        int[,] matrix = new int [,] {
            {0,1,2},
            {3,4,5},
            {6,7,8},
            {0,3,6},
            {1,4,7},
            {2,5,8},
            {0,4,8},
            {2,4,6}
        };
        for(int i = 0;i < 8;i++){
            int a = matrix[i,0];
            int b = matrix[i,1];
            int c = matrix[i,2];
            if (markedspaces[a] + markedspaces[b] + markedspaces[c] == -100 +(2 * (whichTurn + 1))) {
                if (markedspaces[a] == -100) return a;
                if (markedspaces[b] == -100) return b;
                if (markedspaces[c] == -100) return c;
            }
        }
        return -100;
    }
    bool WinnerCheck(){
        
        int s1 = markedspaces[0] + markedspaces[1] + markedspaces[2];
        int s2 = markedspaces[3] + markedspaces[4] + markedspaces[5];
        int s3 = markedspaces[6] + markedspaces[7] + markedspaces[8];
        int s4 = markedspaces[0] + markedspaces[3] + markedspaces[6];
        int s5 = markedspaces[1] + markedspaces[4] + markedspaces[7];
        int s6 = markedspaces[2] + markedspaces[5] + markedspaces[8];
        int s7 = markedspaces[0] + markedspaces[4] + markedspaces[8];
        int s8 = markedspaces[2] + markedspaces[4] + markedspaces[6];
        var solutions = new int [] {s1,s2,s3,s4,s5,s6,s7,s8};
        bool flag = false;
        for(int i = 0; i < solutions.Length;i++){
            if(solutions[i] == 3*(whoTurn + 1))
            {
                WinnerDisplay(i);
                flag = true;
                return flag;
            }
        }
        
        if (!flag && turnCount == 9){
            winnerPanel.gameObject.SetActive(true);
            for(int i = 0;i < winnerText.Length;i++){
                winnerText[i].gameObject.SetActive(false);
            }
            winnerText[2].gameObject.SetActive(true);
            winnerText[2].text = "Draw!";
            return true;
        }
        return false;
    }

    void WinnerDisplay(int indexin)
    {
        for(int i = 0;i<tictactoeSpaces.Length;i++){
            tictactoeSpaces[i].interactable = false;
        }
        winnerPanel.gameObject.SetActive(true);
        if(whoTurn == 0){
            xPlayerScore ++;
            xPlayerScoreText.text = xPlayerScore.ToString();
            for(int i = 0;i < winnerText.Length;i++){
                winnerText[i].gameObject.SetActive(false);
            }
            winnerText[0].gameObject.SetActive(true);
            winnerText[0].text = "Player X Wins!";
        }else if (whoTurn == 1){
            oPlayerScore++;
            oPlayerScoreText.text = oPlayerScore.ToString();
            for(int i = 0;i < winnerText.Length;i++){
                winnerText[i].gameObject.SetActive(false);
            }
            winnerText[1].gameObject.SetActive(true);
            winnerText[1].text = "Player O Wins!";
        }
        winningLine[indexin].SetActive(true); 
    }

    public void Rematch(){
        GameSetup();
        for(int i =0; i < winningLine.Length; i ++){
            winningLine[i].SetActive(false);
        }
        winnerPanel.SetActive(false);
        xPlayersButton.interactable = true;
        oPlayresButton.interactable = true;
    }

    public void Restart(){
        Rematch();
        xPlayerScore = 0;
        oPlayerScore = 0;
        xPlayerScoreText.text = "0";
        oPlayerScoreText.text = "0";
    }

    public void SwitchPlayer(int whichPlayer){
        if (whichPlayer == 0){
            whoTurn = 0;
            turnIcons[0].SetActive(true);
            turnIcons[1].SetActive(false);
            
        }else if(whichPlayer == 1){
            whoTurn = 1;
            turnIcons[0].SetActive(false);
            turnIcons[1].SetActive(true);
            if(playWithAI){
                AIMove();
            }
        }
    }
}

