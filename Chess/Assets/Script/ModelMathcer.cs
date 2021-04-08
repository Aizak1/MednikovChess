using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ModelMathcer:MonoBehaviour
{
    [SerializeField] private GameObject whitePawnModel;
    [SerializeField] private GameObject whiteRookModel;
    [SerializeField] private GameObject whiteKnightModel;
    [SerializeField] private GameObject whiteBishopModel;
    [SerializeField] private GameObject whiteQueenModel;
    [SerializeField] private GameObject whiteKingModel;
    [SerializeField] private GameObject blackPawnModel;
    [SerializeField] private GameObject blackRookModel;
    [SerializeField] private GameObject blackKnightModel;
    [SerializeField] private GameObject blackBishopModel;
    [SerializeField] private GameObject blackQueenModel;
    [SerializeField] private GameObject blackKingModel;
    private Dictionary<Tuple<Kind, bool>, GameObject> kindModelPairs;
    public Dictionary<Tuple<Kind, bool>, GameObject> KindModelPairs => kindModelPairs;
    private void Awake()
    {
        //В качестве bool передается значение isWhite 
        kindModelPairs = new Dictionary<Tuple<Kind, bool>, GameObject>()
        {
            {Tuple.Create(Kind.Pawn,true),whitePawnModel },
            {Tuple.Create(Kind.Rook,true),whiteRookModel },
            {Tuple.Create(Kind.Knight,true),whiteKnightModel },
            {Tuple.Create(Kind.Bishop,true),whiteBishopModel },
            {Tuple.Create(Kind.Queen,true),whiteQueenModel },
            {Tuple.Create(Kind.King,true),whiteKingModel },
            {Tuple.Create(Kind.Pawn,false),blackPawnModel },
            {Tuple.Create(Kind.Rook,false),blackRookModel },
            {Tuple.Create(Kind.Knight,false),blackKnightModel },
            {Tuple.Create(Kind.Bishop,false),blackBishopModel },
            {Tuple.Create(Kind.Queen,false),blackQueenModel },
            {Tuple.Create(Kind.King,false),blackKingModel },
        };
    }
}
