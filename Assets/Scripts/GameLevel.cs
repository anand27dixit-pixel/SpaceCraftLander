using UnityEngine;

public class GameLevel : MonoBehaviour
{
    [SerializeField] private int levelNumber = 1;
    [SerializeField] private Transform landerStartPosition;
    [SerializeField] private Transform cinemachineCameraTarget;

    [SerializeField] private float zoomdedOutOrthographicSize;

    public int GetLevelNumber() => levelNumber;

    public float GetZoomedOutOrthographicZie() => zoomdedOutOrthographicSize;

    public Transform GetCineMachineCamTarget() => cinemachineCameraTarget;

    public void SetLanderStartPosition()
    {
        Lander.Instance.transform.position = landerStartPosition.position;
    }

}
