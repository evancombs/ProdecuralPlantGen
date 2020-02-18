using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class LSystem : MonoBehaviour
{
    public int seed;
    
    // If true will constantly destroy and redraw the tree, allowing updates to the traits in real time. Absolutely destroyes preformance.
    public bool liveUpdate = false;
    // If true all traits will be chosen randomly, within a "super range".
    public bool randomTraits = false;

    // Variables that add variation to how the tree looks. These will be
    // randomly chosen and varied.
    public int iterations;
    public float angle;
    public float leafWidth;
    public float minLeafLength;
    public float maxLeafLength;
    public float branchWidth;
    public float minBranchLength;
    public float maxBranchLength;
    public float variance;

    public Color32 leafColor;
    public Color32 branchColor;

    // Prefabs that the plant is made of. These can be stretch/distorted depending on the randomly chosen variables.
    public GameObject tree;
    public GameObject branch;
    public GameObject leaf;

    private const string axiom = "X";

    // This Dictionary simply stores the rules for each character, defined by the LSystem. 
    private Dictionary<char, string> rules = new Dictionary<char, string>();
    // This stack is used to save transforms for when it is required to return to a previous state, simulating recursion.
    private Stack<SavedTransform> savedTransforms = new Stack<SavedTransform>();
    // The position that the LSystem starts at.
    private Vector3 initalPosition;

    private Dictionary<char, List<string>> randomRules = new Dictionary<char, List<string>>();

    private string currentPath = "";
    private float[] randomRotations;

    System.Random rand;

    private int leafCount = 0;
    private int branchCount = 0;
    // Runs when the program is validated in Unity 
    private void Awake()
    {
        // If no seed is supplied, Unity makes it 0. 0 is considered to be unseeded, and thus we will have a random seed.
        rand = new System.Random();
        if (seed == 0)
        {
            seed = rand.Next();
        }
        rand = new System.Random(seed);
        
        //rand = new System.Random();
        randomRotations = new float[1000];
        for (int i = 0; i < randomRotations.Length; i++)
        {
            //randomRotations[i] = Random.Range(-.1f, .1f);
            // * (max - min) + min
            randomRotations[i] = ((float) rand.NextDouble()) * ((0.1f) - (-0.1f) - (-0.1f));
        }
        if (randomTraits)
            chooseRandomTraits();
        chooseRules();
        //rand = new System.Random();
        //Generate();
        GenerateAlt();
    }

    private void chooseRules()
    {
        //rules.Add('X', "[-FX][+FX][FX]");
        rules.Add('X', "[F-[[X]+X]+F[+FX]-X]");
        rules.Add('F', "FF");

        // bruh moment
        // completeRules will be a list of lists of possible rules. For example:
        // Index 0 of completeRules contains a list of possible rules that will generate a particular style of plant,
        // Index 1 contains another list of possible rules, etc.
        
        List<List<string>> completeRules = new List<List<string>>();

        // Possible rule sets are defined here, using add statements
        completeRules.Add
            (
            
                new List<string>
                {
                    "[F-[[/X]+*X]+/F[+*FX]-X]",
                    "[F+[[/X*]-X*]-/F[-FX]+X]"
                }
             );
        /*completeRules.Add
            (
                new List<string>
                {
                    "[F-[[/X]+*X]+/F[*F+FX]-X]",
                    "[+F[[*+X]+X/]-*F[/FX]+X]"
                }
            );
            */
        List<string> tempRules = new List<string>
        {
            "[F-[[/X]+*X]+/F[+*FX]-X]",
            "[F+[[/X*]-X*]-/F[-FX]+X]"
            //"[[F+[X]-X++][-F+X]+F]"
        };
        // Here we choose an index of completeRules, chosing which rule set we are going to use for generation.
        int chosenCompleteRulesListIndex = rand.Next(completeRules.Count);
        randomRules.Add('X', completeRules[chosenCompleteRulesListIndex]);
        randomRules.Add('F', new List<string> { "FF" });
    }

    private void chooseRandomTraits()
    {
        angle = (float) rand.NextDouble() * (30);
        //width = (float) rand.NextDouble() * (10);
        leafWidth = (float) rand.NextDouble() * (50); ;
        minLeafLength = (float) rand.NextDouble() * (50); 
        maxLeafLength = ((float)rand.NextDouble() * (50)) + minLeafLength;
        branchWidth = (float)rand.NextDouble() * (10); ;
        minBranchLength = (float)rand.NextDouble() * (5); ;
        maxBranchLength = ((float)rand.NextDouble() * (5)) + minBranchLength; ;
        variance = (float)rand.NextDouble() * (100); ;

        leafColor = new Color32
            (
                (byte) rand.Next(255),
                (byte) rand.Next(255),
                (byte) rand.Next(255),
                255
            );
        branchColor = new Color32
            (
                (byte)rand.Next(255),
                (byte)rand.Next(255),
                (byte)rand.Next(255),
                255
            );
    }

    private void Generate()
    {
        // The first time Generate() is called, the axiom is the starting point
        currentPath = axiom;

        // Similar to a string that can be edited and changed easily
        StringBuilder stringBuilder = new StringBuilder();

        // This is about to get wild but this is where the instructions for the plant are made. OK:
        // The string is processed n times, where n is iterations. For every iterations, the current string is coppied into a temp char array
        // for ease of use. Then a second for loop loops over every character in the array, adding it to the StringBuilder. If a given character 
        // has a rule defined in the dictionary, that is added to the stringbuilder instead. Otherwise, the character is simply added.
        // After each iteration, the string built with the StringBuilder is set back to the current path, and the StringBuilder is reset.
        for (int i = 0; i < iterations; i++)
        {
            char[] currentPathChars = currentPath.ToCharArray();
            for (int j = 0; j < currentPathChars.Length; j++)
            {
                //stringBuilder.Append(rules.ContainsKey())
                if (rules.ContainsKey(currentPathChars[j]))
                    stringBuilder.Append(rules[currentPathChars[j]]);
                else
                    stringBuilder.Append(currentPathChars[j].ToString());
            }
            currentPath = stringBuilder.ToString();
            stringBuilder = new StringBuilder();
        }
        // At this point the string of instructions for the LSystem is now complete, and the next step is to draw it according to the instructions.
        Debug.Log("Path is now: " + currentPath);
        //Draw();
        DrawAlt();
        
    }

    private void GenerateAlt()
    {
        currentPath = axiom;

        // Similar to a string that can be edited and changed easily
        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < iterations; i++)
        {
            char[] currentPathChars = currentPath.ToCharArray();
            for (int j = 0; j < currentPathChars.Length; j++)
            {
                //stringBuilder.Append(rules.ContainsKey())
                if (rules.ContainsKey(currentPathChars[j]))
                {
                    int randomRuleInt = rand.Next(randomRules[currentPathChars[j]].Count);
                    stringBuilder.Append(randomRules[currentPathChars[j]][randomRuleInt]);
                }
                else
                {
                    stringBuilder.Append(currentPathChars[j].ToString());
                }
            }
            currentPath = stringBuilder.ToString();
            stringBuilder = new StringBuilder();
        }
        // At this point the string of instructions for the LSystem is now complete, and the next step is to draw it according to the instructions.
        Debug.Log("Path is now: " + currentPath);
        //Draw();
        DrawAlt();

    }


    private void Draw()
    {
        for (int k = 0; k < currentPath.Length; k++)
        {
            switch (currentPath[k])
            {
                // 'F' Means a straight line-- continue on. In this particular implementation, it uses a fancy algorith to 
                case 'F':
                    initalPosition = transform.position;
                    // By default, elements are NOT leaves
                    bool isLeaf = false;

                    GameObject currentElement;
                    // I have no idea how this works, but it determines if the next element to draw should be a leaf or a branch.
                    if (currentPath[k + 1] % currentPath.Length == 'X' || currentPath[k + 3] % currentPath.Length == 'F' && currentPath[k + 4] % currentPath.Length == 'X')
                    {
                        currentElement = Instantiate(leaf);
                        isLeaf = true;
                    }
                    else
                    {
                        currentElement = Instantiate(branch);
                    }

                    // Make the newest element drawn inherit from the tree prefab (???)
                    currentElement.transform.SetParent(tree.transform);

                    TreeElement currentTreeElement = currentElement.GetComponent<TreeElement>();

                    if (isLeaf)
                    {
                        transform.Translate(Vector3.up * 2f * Random.Range(minLeafLength, maxLeafLength));
                        currentTreeElement.GetComponent<LineRenderer>().startWidth = 0;
                        currentTreeElement.GetComponent<LineRenderer>().endWidth = leafWidth;
                        leafCount++;
                    }
                    else
                    {
                        transform.Translate(Vector3.up * 2f * Random.Range(minBranchLength, maxBranchLength));
                        currentTreeElement.lineRenderer.startWidth = branchWidth;
                        currentTreeElement.lineRenderer.endWidth = branchWidth;
                        branchCount++;
                    }
                    currentTreeElement.lineRenderer.SetPosition(0, initalPosition);
                    currentTreeElement.lineRenderer.SetPosition(1, transform.position);
                    currentTreeElement.lineRenderer.sharedMaterial = currentTreeElement.material;
                    break;

                case 'X':
                    break;

                case '+':
                    transform.Rotate(Vector3.forward * angle * (1f + variance / 10f * randomRotations[k % randomRotations.Length]));
                    break;
                case '-':
                    transform.Rotate(Vector3.back * angle * (1f + variance / 10f * randomRotations[k % randomRotations.Length]));
                    break;
                case '*':
                    transform.Rotate(Vector3.up * 120f * (1f + variance / 10f * randomRotations[k % randomRotations.Length]));
                    break;
                case '/':
                    transform.Rotate(Vector3.down * 120f * (1f + variance / 10f * randomRotations[k % randomRotations.Length]));
                    break;
                case '[':
                    SavedTransform savedTransformPush = new SavedTransform();
                    savedTransformPush.position = transform.position;
                    savedTransformPush.rotation = transform.rotation;                    
                    savedTransforms.Push(savedTransformPush);
                    break;
                case ']':
                    SavedTransform savedTransformPop = savedTransforms.Pop();

                    transform.position = savedTransformPop.position;
                    transform.rotation = savedTransformPop.rotation;
                    break;

            }
        }
        Debug.Log("leafCount : " + leafCount + " , branchCount : " + branchCount);
    }
    private void DrawAlt()
    {
        for (int k = 0; k < currentPath.Length; k++)
        {
            switch (currentPath[k])
            {
                // 'F' Means a straight line-- continue on.
                case 'F':
                    initalPosition = transform.position;

                    GameObject treeSegment;
                    
                    bool isLeaf = false;
                    if (currentPath[k + 1] % currentPath.Length == 'X' || currentPath[k + 3] % currentPath.Length == 'F' && currentPath[k + 4] % currentPath.Length == 'X')
                    {
                        treeSegment = Instantiate(leaf);
                        isLeaf = true;
                    }
                    else
                    {
                        treeSegment = Instantiate(branch);
                    }
                    //treeSegment.transform.SetParent(gameObject.transform);
                    //transform.position = initalPosition;
                    treeSegment.transform.SetParent(gameObject.transform, false);
                    transform.Translate(Vector3.up * maxBranchLength);
                    //this.SetParent(treeSegment.transform, transform.parent);
                    //GameObject treeSegment = Instantiate(branch);
                    if (isLeaf)
                    {
                        transform.Translate(Vector3.up * Random.Range(minLeafLength, maxLeafLength));
                        treeSegment.GetComponent<LineRenderer>().SetPosition(0, initalPosition);
                        treeSegment.GetComponent<LineRenderer>().SetPosition(1, transform.position);
                        treeSegment.GetComponent<LineRenderer>().startWidth = leafWidth;
                        treeSegment.GetComponent<LineRenderer>().endWidth = 0;
                        //treeSegment.GetComponent<LineRenderer>().SetColors(leafColor, leafColor);
                        treeSegment.GetComponent<LineRenderer>().startColor = leafColor;
                        treeSegment.GetComponent<LineRenderer>().endColor = leafColor;
                    }
                    else
                    {
                        transform.Translate(Vector3.up * Random.Range(minBranchLength, maxBranchLength));
                        treeSegment.GetComponent<LineRenderer>().SetPosition(0, initalPosition);
                        treeSegment.GetComponent<LineRenderer>().SetPosition(1, transform.position);
                        treeSegment.GetComponent<LineRenderer>().startWidth = branchWidth;
                        treeSegment.GetComponent<LineRenderer>().endWidth = branchWidth;
                        treeSegment.GetComponent<LineRenderer>().startColor = branchColor;
                        treeSegment.GetComponent<LineRenderer>().endColor = branchColor;
                    }
                    break;

                case 'X': // Do nothing, its just part of the algorithm
                    break;

                case '+': // Rotate clockwise
                    transform.Rotate(Vector3.back * angle * (1f + variance / 10f * randomRotations[k % randomRotations.Length]));
                    break;

                case '-': // Rotate counter-clockwise
                    transform.Rotate(Vector3.forward * angle * (1f + variance / 10f * randomRotations[k % randomRotations.Length]));
                    break;

                case '*':
                    transform.Rotate(Vector3.up * 120f * (1f + variance / 10f * randomRotations[k % randomRotations.Length]));
                    break;
                case '/':
                    transform.Rotate(Vector3.down * 120f * (1f + variance / 10f * randomRotations[k % randomRotations.Length]));
                    break;

                case '[': // Save current transform to stack
                    savedTransforms.Push(new SavedTransform()
                    {
                        position = transform.position,
                        rotation = transform.rotation
                    });
                    break;

                case ']': // Pop saved transform from stack
                    SavedTransform savedTransform = savedTransforms.Pop();
                    transform.position = savedTransform.position;
                    transform.rotation = savedTransform.rotation;
                    break;

            }
        }
    }

    public void SetParent(Transform thisChild, Transform thisParent)
    {
        thisChild.SetParent(thisParent);
        thisChild.localPosition = Vector3.zero;
        thisChild.localRotation = Quaternion.identity;
        thisChild.localScale = Vector3.one;
    }
    void Update()
    {
        if (liveUpdate)
        {
            foreach (Transform child in transform)
            {
                
                Destroy(child.gameObject);

            }
            DrawAlt();
        }
    }
}
