import React from 'react';
import { Slide, Alert, Snackbar } from '@mui/material';
import useMessageProvider from '../useMessageProvider';

const ErrorMessage = () => {

    const { error, removeError } = useMessageProvider();

    return (
        <Snackbar TransitionComponent={Slide} open={!!error} autoHideDuration={3000} onClose={removeError}>
            <Alert variant='filled' onClose={removeError} severity={'error'}>
                {error}
            </Alert>
        </Snackbar>
    )
}

export default ErrorMessage;