import { useContext } from 'react';
import MessageProvider, { MessageContext } from './MessageProvider';

function useMessageProvider() {
    const { message, addMessage, removeMessage, error, addError, removeError } = useContext(MessageContext)
    return { message, addMessage, removeMessage, error, addError, removeError }
}

export default useMessageProvider;