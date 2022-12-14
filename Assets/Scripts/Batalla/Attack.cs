using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private TipoAtaque tipoAtaque;
    [SerializeField] private ParticleSystem particulasCurar;
    [SerializeField] private List<AudioClip> efectosSonido;

    void Start()
    {
        tipoAtaque = GetComponent<PlayerController>().getPersonaje().GetTipoAtaque();
    }

    

    public void performAttack(GridManager grid, Celda objetivo)
    {
        if(objetivo != null && grid != null)
        {
            switch (tipoAtaque)
            {
                case TipoAtaque.SINGLE:
                    singleAttack(objetivo);
                    GameManager.instance.GetAudioSource().PlayOneShot(efectosSonido[0]);
                    break;
                case TipoAtaque.COLUMN:
                    columnAttack(grid, objetivo);
                    GameManager.instance.GetAudioSource().PlayOneShot(efectosSonido[0]);
                    break;
                case TipoAtaque.ROW:
                    rowAttack(grid, objetivo);
                    GameManager.instance.GetAudioSource().PlayOneShot(efectosSonido[0]);
                    break;
                case TipoAtaque.GRID:
                    gridAttack(grid);
                    GameManager.instance.GetAudioSource().PlayOneShot(efectosSonido[0]);
                    break;
                case TipoAtaque.HEAL:
                    healAttack(objetivo);
                    GameManager.instance.GetAudioSource().PlayOneShot(efectosSonido[1]);
                    break;
            }
        }
        
    }

    public void singleAttack(Celda celda) 
    {
        var damage = GetComponent<PlayerController>().getPersonaje().GetAtaque();

        
        if(celda.GetPersonaje() != null)
        {
            var enemigo = celda.GetPersonaje();
            var defEnemigo = enemigo.GetComponent<EnemigoController>().getEnemigo().GetDefensa();
            var damageTotal = damage * Random.Range(1.05f, 1.1f);
            if (damage - defEnemigo > 0)
            {
                if(enemigo.GetComponent<EnemigoController>().getEnemigo().GetTipoAtaque() == TipoAtaque.ROW)
                {
                    enemigo.GetComponent<EnemigoController>().getEnemigo().takeDamage(damageTotal + damage * 0.1f - defEnemigo / 2);
                }
                else
                {
                    enemigo.GetComponent<EnemigoController>().getEnemigo().takeDamage(damageTotal - defEnemigo / 2);
                }
            }
            else
            {
                enemigo.GetComponent<EnemigoController>().getEnemigo().takeDamage(damageTotal * 3 / 5);
            }
                

        }
        
    }

    public void columnAttack(GridManager grid, Celda objetivo) 
    {

        var damage = GetComponent<PlayerController>().getPersonaje().GetAtaque();
        Debug.Log(objetivo.GetY());
        foreach (var celda in grid.getGridInfo().getColumn(objetivo.GetY()))
        {
            if(celda.GetPersonaje() != null)
            {
                var enemigo = celda.GetPersonaje();
                var defEnemigo = enemigo.GetComponent<EnemigoController>().getEnemigo().GetDefensa();
                if (damage - defEnemigo > 0)
                {
                    if (enemigo.GetComponent<EnemigoController>().getEnemigo().GetTipoAtaque() == TipoAtaque.GRID)
                    {
                        enemigo.GetComponent<EnemigoController>().getEnemigo().takeDamage(damage + damage * 0.1f - defEnemigo / 2);
                    }
                    else
                    {
                        enemigo.GetComponent<EnemigoController>().getEnemigo().takeDamage(damage - defEnemigo / 2);
                    }
                }
                else
                {
                    enemigo.GetComponent<EnemigoController>().getEnemigo().takeDamage(damage * 3 / 5);
                }
                    
            }
            
        }
    }

    public void rowAttack(GridManager grid, Celda objetivo) 
    {
        var damage = GetComponent<PlayerController>().getPersonaje().GetAtaque();
        
        foreach (var celda in grid.getGridInfo().getRow(objetivo.GetX()))
        {
            if (celda.GetPersonaje() != null)
            {
                var enemigo = celda.GetPersonaje();
                var defEnemigo = enemigo.GetComponent<EnemigoController>().getEnemigo().GetDefensa();
                if (damage - defEnemigo > 0)
                {
                    if (enemigo.GetComponent<EnemigoController>().getEnemigo().GetTipoAtaque() == TipoAtaque.COLUMN)
                    {
                        enemigo.GetComponent<EnemigoController>().getEnemigo().takeDamage(damage + damage * 0.1f - defEnemigo / 2);
                    }
                    else
                    {
                        enemigo.GetComponent<EnemigoController>().getEnemigo().takeDamage(damage - defEnemigo / 2);
                    }
                }
                else
                {
                    enemigo.GetComponent<EnemigoController>().getEnemigo().takeDamage(damage * 3 / 5);
                }
            }
            
        }
    }

    public void gridAttack(GridManager grid) 
    {
        var damage = GetComponent<PlayerController>().getPersonaje().GetAtaque();
        foreach (var celda in grid.getGridInfo().GetCeldas())
        {
            
            if(celda.GetPersonaje() != null)
            {
                var enemigo = celda.GetPersonaje();
                var defEnemigo = enemigo.GetComponent<EnemigoController>().getEnemigo().GetDefensa();
                var damageTotal = damage * Random.Range(0.9f, 0.95f);
                if (damage - defEnemigo > 0)
                {
                    if (enemigo.GetComponent<EnemigoController>().getEnemigo().GetTipoAtaque() == TipoAtaque.SINGLE)
                    {
                        enemigo.GetComponent<EnemigoController>().getEnemigo().takeDamage(damageTotal + damage*0.1f - defEnemigo / 2);
                    }
                    else
                    {
                        enemigo.GetComponent<EnemigoController>().getEnemigo().takeDamage(damageTotal - defEnemigo / 2);
                    }
                }
                else
                {
                    enemigo.GetComponent<EnemigoController>().getEnemigo().takeDamage(damage * 3 / 5);
                }
            }
            
        }
    }

    public void healAttack(Celda celda) 
    {
        var damage = GetComponent<PlayerController>().getPersonaje().GetAtaque();

        if (celda.GetPersonaje() != null)
        {
            var aliado = celda.GetPersonaje();
            var damageTotal = damage * Random.Range(0.35f, 0.5f);
            var particles = Instantiate(particulasCurar, aliado.transform.position, Quaternion.Euler(-90, 0, 0));
            particles.transform.parent = aliado.transform;
            particles.transform.localScale = Vector3.one;
            particles.Play();
            aliado.GetComponent<PlayerController>().getPersonaje().curar(damageTotal);

        }
    }


}
