using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

	public NavMeshAgent agent; 

	public GameObject point; 
	public GameObject line; 

	public float distance = 1; 
	public float height = 0.01f;

	private List<GameObject> points;
	private Vector3 agentPoint;
	private Vector3 lastPoint;
	private List<GameObject> lines;

	private bool isGameActive;
	private bool selected;

	private int scoreCoins;
	public Text scoreText;
	public Text gameOverText;
	public Text youWinText;

	public Button restartButton;

	void Awake()
	{
		points = new List<GameObject>();
		lines = new List<GameObject>();
		UpdatePath();
	}

    void Start()
    {
		scoreCoins = 0;
		UpdateScore(0);

		isGameActive = true;
	}

    void ClearArray() 
	{
		foreach (GameObject obj in points)
		{
			Destroy(obj);
		}
		foreach (GameObject obj in lines)
		{
			Destroy(obj);
		}
		lines = new List<GameObject>();
		points = new List<GameObject>();
	}

	bool IsDistance(Vector3 distancePoint)
	{
		bool result = false;
		float dis = Vector3.Distance(lastPoint, distancePoint);
		if (dis > distance) result = true;
		lastPoint = distancePoint;
		return result;
	}

	void UpdatePath() 
	{
		lastPoint = agent.transform.position + Vector3.forward * 100f; 

		ClearArray();

		for (int j = 0; j < agent.path.corners.Length; j++)
		{
			if (IsDistance(agent.path.corners[j]))
			{
				GameObject p = Instantiate(point) as GameObject;
				p.transform.position = agent.path.corners[j] + Vector3.up * height; 
				points.Add(p);
			}
		}

		for (int j = 0; j < points.Count; j++)
		{
			if (j + 1 < points.Count)
			{
				Vector3 center = (points[j].transform.position + points[j + 1].transform.position) / 2;
				Vector3 vec = points[j].transform.position - points[j + 1].transform.position;
				float dis = Vector3.Distance(points[j].transform.position, points[j + 1].transform.position); 

				GameObject p = Instantiate(line) as GameObject;
				p.transform.position = center - Vector3.up * height;
				p.transform.rotation = Quaternion.FromToRotation(Vector3.right, vec.normalized); 
				p.transform.localScale = new Vector3(dis, 1, 1);
				lines.Add(p);
			}
		}
	}

	void Update()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		
		if (Physics.Raycast(ray, out hit))
		{
			if (Input.GetMouseButtonDown(0) && !selected && isGameActive)
			{
				agent.SetDestination(hit.point);
				selected = true;
			}
		}

		

		if (agentPoint != agent.path.corners[agent.path.corners.Length - 1]) UpdatePath(); 
		agentPoint = agent.path.corners[agent.path.corners.Length - 1];

		if (agent.path.corners.Length == 1 && points.Count > 1)
		{
			UpdatePath();
			selected = false;
		}
		
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Spike"))
		{
			Destroy(gameObject);
			GameOver();
		}

		if (collision.gameObject.CompareTag("Coin"))
		{
			scoreCoins++;
			UpdateScore(scoreCoins);
			Destroy(collision.gameObject);
			if (scoreCoins >= 6)
            {
				isGameActive = true;
				YouWin();
            }
		}
	}

	public void UpdateScore(int scoreToAdd)
	{
		scoreText.text = "Score: " + scoreToAdd;
	}

	public void GameOver()
	{
		gameOverText.gameObject.SetActive(true);
		isGameActive = false;
		restartButton.gameObject.SetActive(true);
	}

	public void YouWin()
	{
		youWinText.gameObject.SetActive(true);
		isGameActive = false;
		restartButton.gameObject.SetActive(true);
	}
}
