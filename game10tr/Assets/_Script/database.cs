using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class database : MonoBehaviour
{
    private DatabaseReference reference;
    private void Awake()
    {
        FirebaseApp app = FirebaseApp.DefaultInstance;
        reference=FirebaseDatabase.DefaultInstance.RootReference;
    }
    private void Start()
    {
        positions pos = new positions(1, 2, positionsType.ground);
        wirteDataBase("h1234", pos.ToString()); // Example usage
        readDataBase("h1234"); // Example usage
    }
    public void wirteDataBase(string id, string message)
    {
        reference.Child("User").Child(id).SetValueAsync(message).ContinueWithOnMainThread(task => 
        {
            if (task.IsCompleted)
            {
                Debug.Log("Data written successfully");
            }
            else
            {
                Debug.LogError("Failed to write data: " + task.Exception);
            }
        });

    }
    public void readDataBase(string id)
    {
        reference.Child("User").Child(id).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    string message = snapshot.Value.ToString();
                    Debug.Log("Data read successfully: " + message);
                }
                else
                {
                    Debug.Log("No data found for the given ID");
                }
            }
            else
            {
                Debug.LogError("Failed to read data: " + task.Exception);
            }
        });
    }
}
