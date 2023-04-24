import React, { useCallback } from 'react';

export const MessageContext = React.createContext({
    message: null,
    addMessage: () => { },
    removeMessage: () => { },
    error: null,
    addError: () => { },
    removeError: () => { }

})

const MessageProvider = ({ children }) => {

    const [message, setMessage] = React.useState(null);

    const addMessage = (message) => setMessage(message);

    const removeMessage = () => setMessage(null);

    const [error, setError] = React.useState(null);

    const addError = (error) => setError(error);

    const removeError = () => setError(null);

    const contextValue = {
        message,
        addMessage: React.useCallback((message) => addMessage(message), []),
        removeMessage: React.useCallback(() => removeMessage(), []),
        error,
        addError: React.useCallback((error) => addError(error), []),
        removeError: React.useCallback(() => removeError(), [])
    }

    return (
        <MessageContext.Provider value={contextValue}>
            {children}
        </MessageContext.Provider>
    )
}

export default MessageProvider;