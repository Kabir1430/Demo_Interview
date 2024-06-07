using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene: MonoBehaviour
{
    // Start is called before the first frame update

  //  public Player p;
    public void Play() {
        SceneManager.LoadScene(1);
    }
    public void Exit()
    {
        Application.Quit();
    }




}
