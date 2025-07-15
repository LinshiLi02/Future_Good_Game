# LLM Integration Setup Guide

## Overview
This system integrates OpenAI's GPT models with Unity NPCs for dynamic conversations. The system is designed to be simple, reusable, and easily configurable.

## Files Created/Modified

### 1. APISettings.cs
- **Purpose**: Centralized configuration for API settings and NPC prompts
- **Features**:
  - API key management
  - Model selection (gpt-4, gpt-3.5-turbo)
  - System prompt configuration
  - Response parameters (max tokens, temperature)
  - Singleton pattern for easy access

### 2. LLMService.cs
- **Purpose**: Handles API communication with OpenAI
- **Features**:
  - Conversation history management
  - Async API calls using UnityWebRequest
  - Event-based response handling
  - Error handling

### 3. Talk.cs (Modified)
- **Purpose**: Enhanced NPC interaction with LLM integration
- **New Features**:
  - Input field for player messages
  - Send/Close buttons
  - Real-time conversation display
  - Automatic initial message

## Setup Instructions

### Step 1: Create API Settings GameObject
1. Create an empty GameObject in your scene
2. Name it "APISettings"
3. Add the `APISettings` component
4. Configure your API key and other settings in the inspector

### Step 2: Set Up UI Elements
For each NPC that uses the Talk script, you'll need these UI elements:
- **InputField**: For player text input
- **Text**: To display conversation
- **Button (Send)**: To send messages
- **Button (Close)**: To close dialog

### Step 3: Configure Talk Script
1. Assign the UI elements to the Talk script in the inspector
2. The script will automatically find or create an LLMService

### Step 4: Test the System
1. Enter your API key in APISettings
2. Play the scene
3. Approach an NPC with the Talk script
4. Press R to start conversation
5. Type messages and press Enter or click Send

## Customization

### Changing NPC Prompts
To use different prompts for different NPCs:
```csharp
// In your script
APISettings.Instance.SetSystemPrompt("Your new prompt here");
```

### Adding New NPCs
1. Create a new GameObject with the Talk script
2. Set up the required UI elements
3. Configure the trigger collider
4. Optionally modify the prompt for this specific NPC

### Security Notes
- Never commit API keys to version control
- Consider using environment variables or encrypted storage
- The SetAPIKey method allows runtime key updates

## Troubleshooting

### Common Issues
1. **"API Settings not found"**: Make sure APISettings GameObject exists in scene
2. **"API Error"**: Check your API key and internet connection
3. **UI not responding**: Verify all UI elements are assigned in inspector

### Debug Tips
- Check the console for error messages
- Use `LLMService.GetConversationHistory()` to debug conversations
- Verify API key format and permissions

## Example Usage

```csharp
// Get API settings
string apiKey = APISettings.Instance.apiKey;

// Change prompt for different NPC
APISettings.Instance.SetSystemPrompt("You are a wise wizard...");

// Clear conversation history
llmService.ClearConversation();
```

## Performance Considerations
- Responses are cached in conversation history
- Consider clearing history for long sessions
- Monitor API usage and costs
- Use appropriate max_tokens to control response length 