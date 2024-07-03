using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WheelRouletteUI : MonoBehaviour
{
    [SerializeField] private Transform wheelBody;
    private float rotateDuration = 5f;    // Duration of the initial rotation
    private float slowdownDuration = 3f;  // Duration of the slowdown phase
    public int targetSegment = 0;        // The segment where the wheel should stop (0 to 7)
	private int defaultTargetSegment = 1;

    private float segmentAngle = 45f;    // Each segment is 45 degrees for an 8-segment wheel
    private bool isRotating = false;


	

	[Header("Wheel UI")]
	[SerializeField] private Image[] all_img_RewardIcons;
	[SerializeField] private TextMeshProUGUI[] all_txt_RewardValue;
	[SerializeField] private TextMeshProUGUI txt_SpinHeader;
	[SerializeField] private TextMeshProUGUI txt_TimeLeft;
	[SerializeField] private Image img_SpinMethodIcon;
	[SerializeField] private Sprite[] all_sprite_SpinMethod;


	[Header("animation")]
	[SerializeField] private Transform rect_main;
	[SerializeField] private float flt_AnimationTime;
	[SerializeField] private float flt_ClosedAnimation;


	private void OnEnable()
	{
		SetWheelSpinAvailability();

		Panel_Animation.instance.Enable_PopUp(rect_main, flt_AnimationTime);
	}

	private void Start()
	{
		SetWheelData();
	}

	private void Update()
	{
		//if (!isRotating) // Need to check this so we do not show WAIT screens right after spin is clicked. We will show WAIT screens after spin is completed.
		//{
			if (!RewardsManager.Instance.wheelRouletteRewardData.IsWheelRouletteActive())
			{
				// if wheel is not active, show timer.
				TimeSpan timeLeftInUnlockingProcess = RewardsManager.Instance.wheelRouletteRewardData.GetCurrentTimeLeft();
				string formattedTime = UtilityManager.Instance.FormatTimeToString(timeLeftInUnlockingProcess);
				txt_TimeLeft.text = formattedTime;
			}
		//}
	}

	public void SetWheelSpinAvailability()
	{
		if (RewardsManager.Instance.wheelRouletteRewardData.IsWheelRouletteActive())
		{
			img_SpinMethodIcon.gameObject.SetActive(true);
			txt_SpinHeader.text = "SPIN!";
			txt_TimeLeft.gameObject.SetActive(false);

			if(DataManager.Instance.skipIts > 0)
			{
				//User has skip its
				img_SpinMethodIcon.sprite = all_sprite_SpinMethod[1];
			}
			else
			{
				img_SpinMethodIcon.sprite = all_sprite_SpinMethod[0];
			}
		}
		else
		{
			img_SpinMethodIcon.gameObject.SetActive(false);
			txt_SpinHeader.text = "WAIT!";
			txt_TimeLeft.gameObject.SetActive(true);
		}
	}

	private void SetWheelData()
	{
		for(int i = 0; i < all_img_RewardIcons.Length; i++)
		{
			all_img_RewardIcons[i].sprite = RewardsManager.Instance.wheelRouletteRewardData.GetRewardIcon(i);
			all_txt_RewardValue[i].text = RewardsManager.Instance.wheelRouletteRewardData.GetRewardAmount(i).ToString();
		}
	}

	private void RotateWheel()
    {
		isRotating = true;


		// Calculate total rotation angle
		float totalRotation = 360f * (rotateDuration + slowdownDuration);  // Full rotations
		totalRotation += segmentAngle * targetSegment;  // Add the angle for the target segment

		// Apply DOTween rotation
		wheelBody.DORotate(new Vector3(0, 0, -totalRotation), rotateDuration + slowdownDuration, RotateMode.FastBeyond360)
			.SetEase(Ease.InOutBack, 0.3f)  // Slow down effect
			.OnComplete(() =>
			{
				//isRotating = false;
				StartCoroutine(ShowWinEffect());
			});
	}

	private IEnumerator ShowWinEffect()
	{
		yield return new WaitForSeconds(2f);	
		isRotating = false;
		SetWheelSpinAvailability();
	}	

	private void RandomizeWhereToStop()
	{
		targetSegment = defaultTargetSegment;

		List<int> list_Randomized = new List<int>();

		for(int i = 0; i < all_img_RewardIcons.Length; i++)
		{
			list_Randomized.Add(i);
		}

		list_Randomized = Shuffle(list_Randomized);		

		for(int i = 0; i < list_Randomized.Count; i++)
		{
			Debug.Log("list randomized value : " + list_Randomized[i]);

			int randomValue = Random.Range(0, 100);
			if (randomValue < RewardsManager.Instance.wheelRouletteRewardData.GetRewardProbability(list_Randomized[i])) 
			{
				targetSegment = list_Randomized[i];
				Debug.Log("HERE : " + i);
				break;
			}

		}
	}

	public static List<T> Shuffle<T>(List<T> _list)
	{
		for (int i = 0; i < _list.Count; i++)
		{
			T temp = _list[i];
			int randomIndex = Random.Range(i, _list.Count);
			_list[i] = _list[randomIndex];
			_list[randomIndex] = temp;
		}

		return _list;
	}

	public void OnClick_Spin()
	{
		if (!RewardsManager.Instance.wheelRouletteRewardData.IsWheelRouletteActive())
		{
			return; // roulette is not active currently.
		}

		if (isRotating)
		{
			return;
		}

		AudioManager.insatance.PlayBtnClickSFX();
		RandomizeWhereToStop();
		RotateWheel();
		img_SpinMethodIcon.gameObject.SetActive(false);
	
		if (DataManager.Instance.skipIts > 0)
		{
			// used skip its to spin the wheel		
			DataManager.Instance.UpdateSkipItsValue(-1);
		}
		else
		{
			RewardsManager.Instance.wheelRouletteRewardData.WheelRouletteClaimed();
			UIManager.Instance.ui_HomeScreen.WheelRouletteClaimed();
		}
		
	}

	public void OnClick_Close()
	{
		AudioManager.insatance.PlayBtnClickSFX();
        Panel_Animation.instance.Disable_PopUp(rect_main, flt_ClosedAnimation , this.gameObject);
    }
}
