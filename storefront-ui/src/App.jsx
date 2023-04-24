import { Grid, Paper, useTheme } from '@mui/material';
import React from 'react';
import { Route, Routes } from 'react-router-dom';
import LandingPage from './components/LandingPage';
import Products from './components/products/Products';
import Stores from './components/stores/Stores';
import Header from './shared/components/Header';
import MessageProvider from './shared/hooks/messageprovider/MessageProvider';
import ErrorMessage from './shared/hooks/messageprovider/messagetypes/ErrorMessage';
import SuccessMessage from './shared/hooks/messageprovider/messagetypes/SuccessMessage';

const App = () => {
    const theme = useTheme();
    return (
        <React.Fragment>
            <MessageProvider>
                <Header />
                <Paper square sx={{
                    minHeight: '100vh',
                    padding: theme.spacing(2)
                }}>
                    <Grid container sx={{
                        marginTop: theme.spacing(2)
                    }}>
                        <Grid item xs={12} sm={12} md={1} />
                        <Grid item xs={12} sm={12} md={10}>
                            <Routes>
                                <Route
                                    path={"/"}
                                    element={<LandingPage />}
                                />
                                <Route
                                    path={"/products/*"}
                                    element={<Products />}
                                />
                                <Route
                                    path={"/stores/*"}
                                    element={<Stores />}
                                />
                            </Routes>
                        </Grid>
                        <Grid item xs={12} sm={12} md={1} />
                    </Grid>
                </Paper>
                <SuccessMessage />
                <ErrorMessage />
            </MessageProvider>
        </React.Fragment>
    )
}
export default App;