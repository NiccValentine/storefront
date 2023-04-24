import { Slide, Alert, Snackbar } from '@mui/material';
import React from 'react';
import useMessageProvider from '../useMessageProvider';

const SuccessMessage = () => {

    const { message, removeMessage } = useMessageProvider();

    return (
        <Snackbar TransitionComponent={Slide} open={!!message} autoHideDuration={3000} onClose={removeMessage}>
            <Alert variant='filled' onClose={removeMessage} severity={'success'}>
                {message}
            </Alert>
        </Snackbar>
    )
}

export default SuccessMessage;