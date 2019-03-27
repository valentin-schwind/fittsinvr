using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TablePosition : MonoBehaviour
{
    public static TablePosition TableInstance;

    public Vector3 A;
    public Vector3 B;
    public Vector3 C;
    public Vector3 Ab;
    public Vector3 Ac;
    public Vector3 Bc;
    public float Heigth;
    //public string filename;
    private float _depth;
    private float _width;
    public GameObject GameController;
    private GameObjects _objects;
    private float _buttonHeight;
    private readonly SaveTablePos _saveTablePos = new SaveTablePos();
    private void Awake()
    {
        TableInstance = this;
    }
    public void GetValues()
    {
        _saveTablePos.LoadFile();
        A = Variables.TopFrontLeft;
        B = Variables.TopFrontRight;
        C = Variables.TopBackLeft;

        Heigth = Variables.TableHeight + _objects.TableTop.transform.localScale.y/2;
    }
    public void SetTableHeight(float h)
    {
        //changeHeigth(h);
        //transform.localScale = new Vector3(depth,h, width);
        
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + h, transform.localPosition.z);
    }
    private void Start()
    {
        _objects = GameController.GetComponent<GameObjects>();
/*        _objects.ButtonsContainer.transform.parent = _objects.TableTop.transform;
        _objects.Buttons.transform.parent = _objects.ButtonsContainer.transform;*/
        GetValues();
        Ab = A - B;
        Ac = A - C;
        Bc = B + C;
        var diag = Bc / 2;

        var mittelpunkt = new Vector3(diag.x, Heigth / 2, diag.z);
        transform.localPosition = mittelpunkt;

        _depth = Mathf.Sqrt(Ac.sqrMagnitude);
        _width = Mathf.Sqrt(Ab.sqrMagnitude);
        transform.localScale = new Vector3(_depth, Heigth, _width);
        _objects.Buttons.transform.position = new Vector3(transform.position.x, transform.position.y * 2, transform.position.z);
    }
    public Vector3 GetTablePos()
    {
        return transform.position;
    }

    public Vector3 GetTableSize()
    {
        return transform.lossyScale;
    }
    private void Update()
    {
        
    }

}
