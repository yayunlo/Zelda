using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour {

	private GameObject ChildGameObject0; 
	private GameObject ChildGameObject1;
	private GameObject ChildGameObject2; 
	private GameObject ChildGameObject3;
	private GameObject ChildGameObject4; 
	private GameObject ChildGameObject5;


	// Use this for initialization
	void Start () {
		ChildGameObject0 = transform.GetChild (0).gameObject; 
		ChildGameObject1 = transform.GetChild (1).gameObject;
		ChildGameObject2 = transform.GetChild (2).gameObject; 
		ChildGameObject3 = transform.GetChild (3).gameObject;
		ChildGameObject4 = transform.GetChild (4).gameObject; 
		ChildGameObject5 = transform.GetChild (5).gameObject;
	}
	
	// Update is called once per frame
	void Update () {

		switch (LinkStatus.health)
		{
			case 0:
				setColor(new Color[] {Color.black, Color.black, Color.black, Color.black, Color.black, Color.black});
				break;
			case 1:
				setColor(new Color[] {Color.black, Color.black, Color.black, Color.black, Color.black, Color.red});
				break;
			case 2:
				setColor(new Color[] {Color.black, Color.black, Color.black, Color.black, Color.red, Color.red});
				break;
			case 3:
				setColor(new Color[] {Color.black, Color.black, Color.black, Color.red, Color.red, Color.red});
				break;
			case 4:
				setColor(new Color[] {Color.black, Color.black, Color.red, Color.red, Color.red, Color.red});
				break;
			case 5:
				setColor(new Color[] {Color.black, Color.red, Color.red, Color.red, Color.red, Color.red});
				break;
			default:
				break;
		}
	}

	void setColor(Color [] colorToSet)
	{
		Image [] healthImage;

		healthImage = ChildGameObject0.GetComponents<Image> ();
		healthImage[0].color = colorToSet[0];
		healthImage = ChildGameObject1.GetComponents<Image> ();
		healthImage[0].color = colorToSet[1];
		healthImage = ChildGameObject2.GetComponents<Image> ();
		healthImage[0].color = colorToSet[2];
		healthImage = ChildGameObject3.GetComponents<Image> ();
		healthImage[0].color = colorToSet[3];
		healthImage = ChildGameObject4.GetComponents<Image> ();
		healthImage[0].color = colorToSet[4];
		healthImage = ChildGameObject5.GetComponents<Image> ();
		healthImage[0].color = colorToSet[5];
	}
}
