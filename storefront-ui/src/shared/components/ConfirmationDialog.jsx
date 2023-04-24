import React from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, DialogContentText, Button } from "@mui/material"

const ConfirmationDialog = ({ open, handleClose, dialogTitle, dialogMessage, confirmationAction }) => {

    return (
        <Dialog
            open={open}
            onClose={handleClose}
            aria-labelledby="delete-dialog-title"
            aria-describedby="delete-dialog-description"
        >
            <DialogTitle id="delete-dialog-title">
                {dialogTitle}
            </DialogTitle>
            <DialogContent>
                <DialogContentText id="delete-dialog-description">
                    {dialogMessage}
                </DialogContentText>
            </DialogContent>
            <DialogActions>
                <Button onClick={handleClose}>Cancel</Button>
                <Button onClick={confirmationAction}>Accept</Button>
            </DialogActions>
        </Dialog>
    )
}

export default ConfirmationDialog;