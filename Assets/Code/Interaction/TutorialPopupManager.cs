using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TutorialPopupManager : MonoBehaviour
{
    [SerializeField] private Image _frame1Object;
    [SerializeField] private Image _frame2Object;
    [SerializeField] private Image _frame3Object;

    [SerializeField] List<TutorialSprites> _tutorialSprites;

    private Dictionary<Tutorial, Sprite[][]> _spriteMap = new();
    public Tutorial CurrentTutorial;
    [SerializeField] private bool _usingGamepad;

    private void OnEnable() => InputSystem.onActionChange += OnActionChange;
    private void OnDisable() => InputSystem.onActionChange -= OnActionChange;

    private void OnActionChange(object obj, InputActionChange change)
    {
        if (change != InputActionChange.ActionPerformed) return;

        var device = ((InputAction)obj).activeControl?.device;
        if (device == null) return;

        bool gamepad = !(device is Keyboard || device is Mouse);
        if (gamepad == _usingGamepad) return;

        _usingGamepad = gamepad;
    }

    private void Awake()
    {
        foreach(TutorialSprites sprites in _tutorialSprites)
        {
            _spriteMap.Add(
                sprites.Type,
                new Sprite[][]
                {
                    sprites.TuotorialImagesController, 
                    sprites.TuotorialImagesKeyboard    
                });
        }
    }

    private void Update()
    {
        _frame1Object.sprite = _usingGamepad ? _spriteMap[CurrentTutorial][0][0] : _spriteMap[CurrentTutorial][1][0];
        _frame2Object.sprite = _usingGamepad ? _spriteMap[CurrentTutorial][0][1] : _spriteMap[CurrentTutorial][1][1];
        _frame3Object.sprite = _usingGamepad ? _spriteMap[CurrentTutorial][0][2] : _spriteMap[CurrentTutorial][1][2];
    }

    [Serializable]
    struct TutorialSprites
    {
        public Tutorial Type;
        public Sprite[] TuotorialImagesController;
        public Sprite[] TuotorialImagesKeyboard;
    }
}
