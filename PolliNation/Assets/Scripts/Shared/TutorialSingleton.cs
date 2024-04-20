using System.Collections.Generic;

public class TutorialSingleton
{
  private static TutorialSingleton _instance;
  public static TutorialSingleton Instance
  {
    get { return _instance; }
  }

  // Milestones
  private static bool _wentOutside;

  public TutorialSingleton()
  {
    if (_instance != null)
    {
      return;
    }
    _instance = this;
  }

  public static void EnteredHive(SnackbarScript snackbarScript)
  {
    if (!_wentOutside)
    {
      snackbarScript.SetText("Head outside to collect resources like pollen and nectar.", 3);
    }
  }

  public static void EnteredOutside()
  {
    _wentOutside = true;
  }

}