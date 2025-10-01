using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
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
    class Move {
        public int row, col; 
    }
    public int[,] changeIndex = {
        {0,1,2},
        {3,4,5},
        {6,7,8}
    };
    public int player = 2, opponent = 1;
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
        int moveIndex = FindBestMoveForAI(1,0);

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
    int FindBestMoveForAI (int AITurn,int playerTurn) // AI turn = 1
    {
        
        if (turnCount == 0 && whoTurn == AITurn){
            return 4;
        }else {
            Move indexMove = findBestMove(markedspaces);
            return changeIndex[indexMove.row,indexMove.col];
        }
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
    bool winnerCheck1()
    {
        int[,] b = changeToMatrix(markedspaces);
        for (int row = 0; row < 3; row++)
        {
            if (b[row, 0] != -100 && b[row, 0] == b[row, 1] && b[row, 1] == b[row, 2])
                return true;
            
        }
        for (int col = 0; col < 3; col++)
        {
            if (b[col, 0] != -100 && b[0, col] == b[1, col] && b[1, col] == b[2, col])
                return true;
        }
        if (b[0, 0] != -100 && b[0, 0] == b[1, 1] && b[1, 1] == b[2, 2])
        {
            return true;
        }

        if (b[0, 2] != -100 && b[0, 2] == b[1, 1] && b[1, 1] == b[2, 0])
        {
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


    // AI min Max
    bool isMoveLeft(int[,] board){
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (board[i, j] == -100)
                    return true;
        return false;
    }
    public int[,] changeToMatrix(int[] board){
        int [,]b = new int [3,3];
        int count = 0;
        for (int i = 0; i < 3;i ++){
            for (int j = 0; j < 3; j++)
            {
                b[i, j] = board[count];
                count++;
            }
        }
        return b;
    }
    int evaluate(int[,] b){
        for(int row = 0;row < 3;row ++){
            if (b[row,0] !=-100 && b[row,0] == b[row,1] && b[row,1] == b[row,2]){
                if(b[row ,0] == player) 
                    return +10;

                else if (b[row,0] == opponent) 
                    return -10;
            }   
        }
        for (int col = 0; col < 3; col++)
        {
            if (b[0,col] != -100 && b[0, col] == b[1, col] &&
                b[1, col] == b[2, col])
            {
                if (b[0, col] == player)
                    return +10;

                else if (b[0, col] == opponent)
                    return -10;
            }
        }
        if (b[0, 0] != -100 && b[0, 0] == b[1, 1] && b[1, 1] == b[2, 2])
        {
            if (b[0, 0] == player)
                return +10;
            else if (b[0, 0] == opponent)
                return -10;
        }

        if (b[0, 2] != -100 && b[0, 2] == b[1, 1] && b[1, 1] == b[2, 0])
        {
            if (b[0, 2] == player)
                return +10;
            else if (b[0, 2] == opponent)
                return -10;
        }
        return 0;
    }
    int minmax(int[,] board,int depth,bool isMax){
        int score = evaluate(board);

        if (score == 10){
            return score - depth ;
        }
        if (score == -10){
            return score + depth ;
        }  
        if(!isMoveLeft(board)){
            return 0;
        }
        if(isMax){
            int best = -1000;
            for(int i = 0; i < 3; i++ ){
                for (int j = 0; j < 3; j ++){
                    if(board[i,j] == -100){
                        board[i,j] = player;
                        best = Math.Max(best,minmax(board,depth + 1,!isMax));
                        board[i,j] = -100;
                    }
                }
            }
            return best;
        }else {
            int best = 1000;
            for(int i = 0; i < 3; i++ ){
                for (int j = 0; j < 3; j ++){
                    if(board[i,j] == -100){
                        board[i,j] = opponent;
                        best = Math.Min(best,minmax(board,depth + 1,!isMax));
                        board[i,j] = -100;
                    }
                }
            }
            return best;
        }
    }
    Move findBestMove(int[] b){
        int [,]board = changeToMatrix(b);
        int bestVal = -1000; 
        Move bestMove = new Move();
        bestMove.row = -1;
        bestMove.col = -1;  
        for(int i = 0;i< 3;i++){
            for(int j = 0; j < 3; j++){
                if(board[i,j] == -100){
                    board[i,j] = player;
                    int moveVal = minmax(board,0,false);
                    board[i,j] = -100;
                    if (moveVal > bestVal){
                        bestMove.row = i;
                        bestMove.col = j;
                        bestVal = moveVal;
                    } 
                }
            }
        }
        return bestMove;
    }
}
