using System.Collections;
using System.Collections.Generic;

using UnityEngine;

// This class acts a wrapper/generator for all data about the plant that will grow from it.
// Given a seed it will determine the traits of the plant and store them, then later can be
// passed to a method to grow a plant from the traits.
public class Seed
{
    private int seed;
    System.Random rand;

    // Enum list to determine the types of traits that required different generation/drawing. 
    // For example shapes of leaves, shape of flower, curvature of stem, etc.
    enum Stems {Curve, Line};
    enum Leaves {Maple, Curly, Round, Long, Spiky};
    enum Flowers {Bulb, Geometric};
    int stemType;
    int leafType;
    int flowerType;

    // Traits common to all varieties of flowers, regardless of their enum traits.
    public float stemThickness;
    public int numLeaves = 10;
    public float plantHeight;

    public Color32 stemColor;
    private Color32 leafColor;
    private Color32 flowerColor;


    // If a seed is passed to the constructor for this class, it generates traits from that seed.
    // If the seed passed is 0, it is considered to be not seeded, and traits are generated from a random seed.
    public Seed(int seed)
    {
        if (seed == 0)
        {
            rand = new System.Random();
            generateTraits(rand.Next());
        }
        else
        {
            this.seed = seed;
            rand = new System.Random(seed);
            generateTraits(seed);
        }
    }


    // Function to call other generation functions, just for organization.
    void generateTraits(int seed)
    {
        // do crazy shit
        generateCommonTraits(seed);

    }

    // Common traits are mostly just traits values within a range, so easy to group together.
    void generateCommonTraits(int seed)
    {
        // Random values within ranges
        stemThickness = (float) rand.NextDouble();
        numLeaves = rand.Next(2, 11);
        plantHeight = (float) rand.NextDouble() * 100;

        // Random colors
        stemColor = new Color32((byte)rand.Next(0, 255), (byte)rand.Next(0, 255), (byte)rand.Next(0, 255), 255);
        leafColor = new Color32((byte)rand.Next(0, 255), (byte)rand.Next(0, 255), (byte)rand.Next(0, 255), 255);
        flowerColor = new Color32((byte)rand.Next(0, 255), (byte)rand.Next(0, 255), (byte)rand.Next(0, 255), 255);
    }

    void generateSpecialTraits(int seed)
    {
        // Pick a random enum value
        stemType = rand.Next(0, System.Enum.GetNames(typeof(Stems)).Length);
        leafType = rand.Next(0, System.Enum.GetNames(typeof(Leaves)).Length);
        flowerType = rand.Next(0, System.Enum.GetNames(typeof(Flowers)).Length);
    }
}
