using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class RatingChance
{
    public int minWave;
    public int maxWave;

    [Range(0, 100)]
    public int commonChance;

    [Range(0, 100)]
    public int rareChance;

    [Range(0, 100)]
    public int epicChance;
}

public class TurretRewardManager : MonoBehaviour
{
    public BundleCard[] bundleCards;

    public List<TurretData> allTurrets;

    public List<RatingChance> ratingChances;
    public CanvasGroup TurretSelectPanelCanvas;
    private Animator TurretSelectPanelCanvasAnimation;
    public CanvasGroup StusPanel;
    void Start()
    {
        TurretSelectPanelCanvasAnimation = TurretSelectPanelCanvas.GetComponent<Animator>();
        if(TurretSelectPanelCanvasAnimation) Debug.Log(TurretSelectPanelCanvasAnimation);
        OpenReward();
    }
    public void OpenReward()
    {
        TurretSelectPanelCanvas.alpha = 1;
        TurretSelectPanelCanvasAnimation.Play("OpenAnimation");
        StusPanel.alpha = 0;
        foreach (BundleCard bundle in bundleCards)
        {
            TurretData a = GetRandomTurret();
            TurretData b = GetRandomTurret();

            bundle.SetData(a, b);
        }
    }
    TurretData GetRandomTurret()
    {
        Rating rating = GetRandomRating();

        List<TurretData> candidates = allTurrets.FindAll(x => x.rating == rating);
        return candidates[Random.Range(0, candidates.Count)];
    }
    RatingChance GetCurrentChance()
    {
        int wave = WaveManager.Instance.currentWave;

        foreach (RatingChance chance in ratingChances)
        {
            if (wave >= chance.minWave &&
                wave <= chance.maxWave)
            {
                return chance;
            }
        }

        return ratingChances[0];
    }
    Rating GetRandomRating()
    {
        RatingChance chance = GetCurrentChance();

        int random = Random.Range(0, 100);

        if (random < chance.commonChance)
        {
            return Rating.Common;
        }

        if (random < chance.commonChance + chance.rareChance)
        {
            return Rating.Rare;
        }

        return Rating.Epic;
    }

    public void SelectBundle(BundleCard bundle)
    {
        GameManager.Instance.AddTurret(bundle.turretA);
        GameManager.Instance.AddTurret(bundle.turretB);

        CloseReward();
    }

    void CloseReward()
    {
        TurretSelectPanelCanvas.alpha = 0;
        TurretSelectPanelCanvasAnimation.SetTrigger("Istr");
        WaveManager.Instance.StartWave();
        StusPanel.alpha = 1;
        
    }
}
