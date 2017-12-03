using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>
/// Global dialog.
/// </summary>
/////////////////////////////////////////////////////////////////////////////////////////////////
public class GlobalDialog : MonoBehaviour {
  // singleton
  static GlobalDialog instance;

  private const string AssetName  = "GlobalDialog";

  [SerializeField] private Canvas canvas;
  [SerializeField] private Text   textContent;
  [SerializeField] private Button buttonLeft;
  [SerializeField] private Button buttonRight;

  // callback
  private Action cbLeftButton;
  private Action cbRightButton;

  public bool IsActive { get { return canvas.enabled; } }

  /////////////////////////////////////////////////////////////////////////////////////////////////
  /// <summary>
  /// Gets the instance.
  /// </summary>
  /// <value>The instance.</value>
  /////////////////////////////////////////////////////////////////////////////////////////////////
  public static GlobalDialog Instance {
    get {
      if (instance == null) {
        Debug.LogError("Dialog is not created!!");
      }
      return instance;
    }
  }

  /////////////////////////////////////////////////////////////////////////////////////////////////
  /// <summary>
  /// Create this instance.
  /// </summary>
  /////////////////////////////////////////////////////////////////////////////////////////////////
  public static Coroutine Create() {
    if (instance != null) {
      return null;
    }
    return GlobalCoroutine.Run(create());
  }

  /////////////////////////////////////////////////////////////////////////////////////////////////
  /// <summary>
  /// Create this instance.
  /// </summary>
  /////////////////////////////////////////////////////////////////////////////////////////////////
  private static IEnumerator create() {

    var req = Resources.LoadAsync(AssetName);
    while (!req.isDone) {
      yield return null;
    }

    GameObject prefab = req.asset as GameObject;
    Instantiate(prefab);
    yield return null;
  }

  /////////////////////////////////////////////////////////////////////////////////////////////////
  /// <summary>
  /// Release this instance.
  /// </summary>
  /////////////////////////////////////////////////////////////////////////////////////////////////
  public static void Release() {
    if (instance != null) {
      Destroy(instance.gameObject);
      instance = null;
    }
  }

  /////////////////////////////////////////////////////////////////////////////////////////////////
  /// <summary>
  /// Awake this instance.
  /// </summary>
  /////////////////////////////////////////////////////////////////////////////////////////////////
  void Awake() {
    if (instance != null) {
      Release();
    }
    instance = this;
    DontDestroyOnLoad(gameObject);

    gameObject.name = "Global_Dialog";

    // set callback
    buttonLeft.onClick.AddListener(onLeftButton);
    buttonRight.onClick.AddListener(onRightButton);

    // disable canvas
    canvas.enabled = false;
  }

  /////////////////////////////////////////////////////////////////////////////////////////////////
  /// <summary>
  /// pushed left button.
  /// </summary>
  /////////////////////////////////////////////////////////////////////////////////////////////////
  private void onLeftButton() {
    if (cbLeftButton != null) {
      cbLeftButton();
    }
    canvas.enabled = false;
  }

  /////////////////////////////////////////////////////////////////////////////////////////////////
  /// <summary>
  /// pushed right button.
  /// </summary>
  /////////////////////////////////////////////////////////////////////////////////////////////////
  private void onRightButton() {
    if (cbRightButton != null) {
      cbRightButton();
    }
    canvas.enabled = false;
  }

  /////////////////////////////////////////////////////////////////////////////////////////////////
  /// <summary>
  /// show dialog.
  /// </summary>
  /// <param name="content">Content.</param>
  /// <param name="cbLeft">Cb left.</param>
  /// <param name="cbRight">Cb right.</param>
  /////////////////////////////////////////////////////////////////////////////////////////////////
  public void Show(string content, Action cbLeft, Action cbRight) {
    // 登録されているリスナーを削除
    cbLeftButton  = null;
    cbRightButton = null;

    // update callback
    cbLeftButton  = cbLeft;
    cbRightButton = cbRight;

    // set content
    textContent.text = content;

    canvas.enabled = true;
  }
}
