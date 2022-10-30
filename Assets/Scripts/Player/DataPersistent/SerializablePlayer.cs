using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class ListaPlayerSerializable
{
    public List<SerializablePlayer> list= new List<SerializablePlayer>();
}

[Serializable]
public class SerializablePlayer
{
    public int flequillo;
    public int pelo;
    public int pestanha;
    public int orejas;
    public int narices;
    public int bocas;
    public int extras;
    public int cejas;
    public int ropa;
    public float rp;
    public float gp;
    public float bp;
    public float ri;
    public float gi;
    public float bi;

    public float ataque;
    public float defensa;
    public float vida;
    public int tipoAtaque;

    public SerializablePlayer() { }

    public SerializablePlayer(int flequillo, int pelo, int pestanha, int orejas, int narices, int bocas, int extras, int cejas, int ropa, float rp, float gp, float bp, float ri, float gi, float bi, float ataque, float defensa, float vida, int tipoAtaque)
    {
        this.flequillo = flequillo;
        this.pelo = pelo;
        this.pestanha = pestanha;
        this.orejas = orejas;
        this.narices = narices;
        this.bocas = bocas;
        this.extras = extras;
        this.cejas = cejas;
        this.ropa = ropa;
        this.rp = rp;
        this.gp = gp;
        this.bp = bp;
        this.ri = ri;
        this.gi = gi;
        this.bi = bi;
        this.ataque = ataque;
        this.defensa = defensa;
        this.vida = vida;
        this.tipoAtaque = tipoAtaque;
    }
}