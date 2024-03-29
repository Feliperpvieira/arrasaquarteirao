﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class CronometroScript : MonoBehaviour
{
    //[Header("Dados para salvar as estrelas")]
    private string codigoFase;
    private string codigoMundo;
    private string codigoMundoFase;

    [Header("Tempo da fase")]
    public float totalTime; //tempo total da fase
    float TimeDuasEstrelas; 
    float TimeTresEstrelas;
    float maximum;

    [Header("Estrelas e Barra de Progresso")]
    public GameObject[] estrelasProgresso;
    public Image barraProgresso;
    static int QuantidadeEstrelas;
    public GameObject[] estrelasVitoria;

    [Header("Menus e jogador")]
    public TMP_Text countdownText;
    public GameObject MenuMorte;
    public GameObject MenuVitoria;
    public Animator CanvasVitoria;
    public GameObject botaoPausa;
    public GameObject Jogador;

    private float minutes;
    private float seconds;

    // Define os tempos para cada estrela na fase
    void Start()
    {
        string nomeDoArquivo = SceneManager.GetActiveScene().name; //Pega o nome da scene !ATENCAO! USAR SEMPRE O FORMATO: W1-Level 0 para isso funcionar

        // Aqui ele vai juntar os trechos do nome da scene da fase pra montar o código, que vai ser usada pra salvar o numero de estrelas no PlayerPrefs
        codigoMundo = "W" + nomeDoArquivo[1];
        if (nomeDoArquivo.Length == 10)
        {
            codigoFase = "L" + nomeDoArquivo[9];
        }
        else if (nomeDoArquivo.Length == 11)
        {
            codigoFase = "L" + nomeDoArquivo[9] + nomeDoArquivo[10];
        }
        codigoMundoFase = codigoMundo + codigoFase;
        Debug.Log(codigoMundoFase);  

        //Time.timeScale = 1f;
        QuantidadeEstrelas = 3;
        maximum = totalTime; // Iguala o maximum (usado na barra de progresso) ao tempo total, mantendo apenas 1 variavel a ser definida no Unity
        TimeTresEstrelas = totalTime - ((totalTime * 45) / 100); //Calcula o tempo que tem para ganhar 3 estrelas (45%)
        TimeDuasEstrelas = totalTime - ((totalTime * 75) / 100); //Calcula o tempo que tem para ganhar 2 estrelas (75%)
    }

    // Update is called once per frame
    public void Update()
    {
        totalTime -= 1 * Time.deltaTime;
        minutes = (int)(totalTime / 60);
        seconds = (int)(totalTime % 60);

        GetCurrentFill();

        countdownText.text = minutes.ToString() + ":" + seconds.ToString("00");

        if (totalTime <= 0)
        {
            totalTime = 0;
            estrelasProgresso[0].SetActive(false);
            estrelasVitoria[0].SetActive(false);
            FimDeJogo();
        }

        //if's para esconder as estrelas amarelas quando o tempo passa
        if(totalTime <= TimeTresEstrelas)
        {
            estrelasProgresso[2].SetActive(false);
            estrelasVitoria[2].SetActive(false);

            QuantidadeEstrelas = 2;
        }
        if (totalTime <= TimeDuasEstrelas)
        {
            estrelasProgresso[1].SetActive(false);
            estrelasVitoria[1].SetActive(false);
            QuantidadeEstrelas = 1;
        }

        if (Pontuacao.score >= Pontuacao.scoreVitoriaStatic)
        {
            Vitoria();
        }
    }

    public void FimDeJogo()
    {
        //Debug.Log("Funcao fim de jogo chamada");
        MenuMorte.SetActive(true);
        Jogador.SetActive(false);
        botaoPausa.SetActive(false);
        Time.timeScale = 0f;
    }

    public void Vitoria()
    {
        MenuVitoria.SetActive(true);
        CanvasVitoria.Play("Base Layer.CanvasVitoria", 0, 0f);
        botaoPausa.SetActive(false);
        Time.timeScale = 0f;

        if (PlayerPrefs.GetInt(codigoMundoFase) == 0) //Faz o passar de nível no levelSelector script
        {
            int atual = PlayerPrefs.GetInt("levelAtual-" + codigoMundo);
            atual = atual + 1;
            PlayerPrefs.SetInt("levelAtual-" + codigoMundo, atual);
        }

        if (QuantidadeEstrelas > PlayerPrefs.GetInt(codigoMundoFase))
        {
            PlayerPrefs.SetInt(codigoMundoFase, QuantidadeEstrelas);
        }

        enabled = false;
    }

    void GetCurrentFill() // Funcao que faz passar o tempo na barra de progresso, ao lado do cronometro
    {
        float fillAmount = (float)totalTime / (float)maximum;
        barraProgresso.fillAmount = fillAmount;
    }
}
