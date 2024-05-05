using UncomplicatedCustomItems.Elements;
using UncomplicatedCustomItems.Elements.SpecificData;
using UncomplicatedCustomItems.Interfaces;
using UncomplicatedCustomItems.Interfaces.SpecificData;
using UnityEngine;

namespace SCP294.CustomItems;
public class Revolver : ICustomItem
{
    public uint Id { get; set; } = 36;

    public string Name { get; set; } = "Revolver di Arthur Morgan";

    public string Description { get; set; } = "";

    public float Weight { get; set; } = 2f;

    public ISpawn Spawn { get; set; } = new Spawn()
    {
        DoSpawn = false,
    };

    public ItemType Item { get; set; } = ItemType.GunRevolver;

    public CustomItemType CustomItemType { get; set; } = CustomItemType.Weapon;

    public Vector3 Scale { get; set; } = Vector3.one;

    public IData CustomData { get; set; } = new WeaponData()
    {
        Damage = 100,
        FireRate = 0.35f,
        MaxAmmo = 100,
    };



}


