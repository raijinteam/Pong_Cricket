using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterName : MonoBehaviour {

    public static CharacterName instance;

    private string[] prefixes = { "Alpha", "Beta", "Gamma", "Delta", "Echo", "Foxtrot", "Gizmo", "Zulu" }; // Add more prefixes as needed
    private string[] suffixes = { "Blade", "Burst", "Nova", "Strike", "Thorn", "Flare", "Fury", "Storm" }; // Add more suffixes as needed

    // List to store generated names
    private List<string> characterNames = new List<string>();

    // Number of names to generate
    private int numberOfNames = 100;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        GenerateNames();
    }



    // Method to generate character names
    private void GenerateNames() {

        for (int i = 0; i < numberOfNames; i++) {
            string name = GenerateRandomName();
            characterNames.Add(name);
        }

        

    }

    // Method to generate a single random name
    public  string GenerateRandomName() {

        // Get random prefix and suffix
        string prefix = prefixes[Random.Range(0, prefixes.Length)];
        string suffix = suffixes[Random.Range(0, suffixes.Length)];

        // Concatenate prefix and suffix to form a name
        string name = prefix + " " + suffix;

        return name;
    }
}
