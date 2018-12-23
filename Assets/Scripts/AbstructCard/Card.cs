using UnityEngine;

abstract public class Card : MonoBehaviour {
    protected string cardName;
    public int cost { get; set; }
    protected Sprite sprite;
    protected string text;

}