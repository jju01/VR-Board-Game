using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

// ���� : ������ �׸��� ��� �浹���� �� ���� Ŭ����

public class MiniGameIce : MonoBehaviour
{
    // �浹�� ��ü ��
    public int count = 0;

    // ȿ����
    //private AudioSource audio;
    // �������� 
    private bool IsPause;

    [Space]
    // �������� �г�
    public GameObject menuPanel;
    public GameObject gameOverPanel;
    public GameObject resultPanel;
    public GameObject itemPanel;

    // start üũ
    public bool isstart = false;

    public bool isend = false;

    public GameObject game;

    public TextMeshProUGUI winnerText;

    private ExitGames.Client.Photon.Hashtable PlayerCustomProperties = new ExitGames.Client.Photon.Hashtable();
    private string miniGameIceKey = "MiniGameIce";

    // �̱������� ���� ����
    public static MiniGameIce Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        gameOverPanel.SetActive(false);
        resultPanel.SetActive(false);
        itemPanel.SetActive(false);
        //IsPause = false;
        //audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // ������ �׸��� �浹���� �� �Լ�
    private void OnTriggerEnter(Collider other)
    {
        if (isend)
        {
            return;
        }

        Ice ice = other.GetComponent<Ice>();
        if (ice.isture)
        {
            ice.isture = false;
            count++;
        }

        // ���� ī��Ʈ �Ǵ� �Լ� ������
        //FindObjectOfType<Ice>().OnTriggerEnter(other);

        // ������ ���(10��) �浹�ϸ�
        if (count == 10)
        {
            if (PlayerCustomProperties.ContainsKey(miniGameIceKey))
            {
                PlayerCustomProperties[miniGameIceKey] = true;
            }
            else
            {
                PlayerCustomProperties.Add(miniGameIceKey, count);
            }
            // ����� ���� ����
            PlayerCustomProperties.Add($"winner2", PhotonNetwork.NickName);

            PhotonNetwork.SetPlayerCustomProperties(PlayerCustomProperties);

            // �̸� �ҷ����� �Լ� ����
            WinnerPlayer(PhotonNetwork.NickName);
            // ���� ���� �Լ�
            OnGameEnd();
        }

    }

    public void OnGameEnd()
    {
        // ���� ����!
        Debug.Log("��������!");
        isend = true;
        // ���� ȭ�� �Ͻ�����
        // PauseGame();
        // UI â ����
        StartCoroutine(OnPanel());
    }

    // ���� ���� & ��� UI â ������ �Լ�
    IEnumerator OnPanel()
    {
        yield return new WaitForSecondsRealtime(0.8f); // WaitForSecondsRealtime = timeScale �� ���� x
        gameOverPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        gameOverPanel.SetActive(false);
        resultPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(2.5f);
        resultPanel.SetActive(false);
        itemPanel.SetActive(true);
    }

    //// �ð� ���� => ���ӳ����� �����Ͻ������ϰ� UIâ�� �����Բ�...
    //void PauseGame()
    //{
    //    if (IsPause == false)
    //    {
    //        Time.timeScale = 0;
    //        IsPause = true;
    //        return;
    //    }
    //}

    // ������â ����
    public void OnClickItemClose()
    {
        itemPanel.SetActive(false);
        Debug.Log("���ξ����� �ް�");
    }

    // ����� �̸� ������ �Լ�
    public void WinnerPlayer(string name)
    {
        winnerText.text = name;
    }
}
