using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokerCardPuzzle : MonoBehaviour
{
    [System.Serializable]
    public class PokerCard
    {
        public int number; // Card value
        public RectTransform cardTransform; // Reference to the card's UI element
        public Button cardButton; // Button component for interaction
    }

    [System.Serializable]
    public class AnswerSlot
    {
        public RectTransform slotTransform; // Slot position in the UI
        public int targetNumber; // Target number (password) for this slot
    }

    [SerializeField] private List<PokerCard> cloversList = new List<PokerCard>();
    [SerializeField] private List<PokerCard> diamondsList = new List<PokerCard>();
    [SerializeField] private List<PokerCard> heartsList = new List<PokerCard>();
    [SerializeField] private List<PokerCard> spadesList = new List<PokerCard>();

    [SerializeField] private List<AnswerSlot> answerSlots; // Slots for the answer list
    [SerializeField] private Button guessButton; // Button to validate the answer
    [SerializeField] private Button resetButton; // Button to reset the puzzle
    [SerializeField] private Text feedbackText; // Feedback for guesses
    [SerializeField] private GameObject puzzleGameObject; // GameObject to disable on completion

    [SerializeField] private CameraLook cl;

    private List<GameObject> answerLineClones = new List<GameObject>(); // Clones in the answer line
    private List<PokerCard> hiddenCards = new List<PokerCard>(); // Hidden cards

    private void Start()
    {
        foreach (var card in GetAllCards())
        {
            card.cardButton.onClick.AddListener(() => AddCardToAnswerLine(card));
        }

        guessButton.onClick.AddListener(CheckAnswer);
        resetButton.onClick.AddListener(ResetPuzzle);
    }

    // Adds a clone of the card to the answer line
    public void AddCardToAnswerLine(PokerCard card)
    {
        if (hiddenCards.Contains(card)) return; // Prevent already hidden cards from being clicked
        if (answerLineClones.Count >= answerSlots.Count) return; // No more space in the answer line

        hiddenCards.Add(card); // Track the card as hidden
        card.cardTransform.gameObject.SetActive(false); // Hide the original card

        // Create a clone and place it in the answer line
        GameObject cardClone = Instantiate(card.cardTransform.gameObject, card.cardTransform.parent);
        cardClone.transform.SetParent(answerSlots[answerLineClones.Count].slotTransform.parent, false); // Keep in the correct hierarchy
        cardClone.GetComponent<RectTransform>().position = answerSlots[answerLineClones.Count].slotTransform.position; // Position in slot
        cardClone.SetActive(true); // Ensure the clone is visible

        // Add a PokerCardInfo component to the clone
        PokerCardInfo cardInfo = cardClone.AddComponent<PokerCardInfo>();
        cardInfo.number = card.number;

        answerLineClones.Add(cardClone); // Add the clone to the answer line list
    }

    // Checks if the answer matches the target numbers
    public void CheckAnswer()
    {
        if (answerLineClones.Count != answerSlots.Count)
        {
            feedbackText.text = "Fill all slots to guess!";
            return;
        }

        for (int i = 0; i < answerSlots.Count; i++)
        {
            PokerCardInfo cloneInfo = answerLineClones[i].GetComponent<PokerCardInfo>();
            if (cloneInfo.number != answerSlots[i].targetNumber)
            {
                feedbackText.text = "Incorrect answer!";
                return;
            }
        }

        feedbackText.text = "Correct answer!";
        if (puzzleGameObject != null)
        {
            puzzleGameObject.SetActive(false); // Disable the puzzle GameObject
            cl.canMove = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Resets the puzzle
    public void ResetPuzzle()
    {
        // Destroy all clones in the answer line
        foreach (var clone in answerLineClones)
        {
            Destroy(clone);
        }

        answerLineClones.Clear(); // Clear the clones list

        // Reveal all hidden cards
        foreach (var card in hiddenCards)
        {
            card.cardTransform.gameObject.SetActive(true);
        }

        hiddenCards.Clear(); // Clear the hidden cards list

        feedbackText.text = ""; // Clear feedback text
    }

    // Helper to get all cards across all suits
    private List<PokerCard> GetAllCards()
    {
        List<PokerCard> allCards = new List<PokerCard>();
        allCards.AddRange(cloversList);
        allCards.AddRange(diamondsList);
        allCards.AddRange(heartsList);
        allCards.AddRange(spadesList);
        return allCards;
    }
}

// Helper script for card information
public class PokerCardInfo : MonoBehaviour
{
    public int number; // Card value
}
