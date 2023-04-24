import { Card, CardContent, CardHeader, Divider, Grid, Typography, useTheme } from '@mui/material';
import React from 'react';
import { useNavigate } from 'react-router-dom';

const LandingPage = () => {

    const navigate = useNavigate();
    const theme = useTheme();

    return (
        <React.Fragment>
            <Typography variant="h5" sx={{
                textAlign: "center"
            }}>
                Thank you for choosing my page! This is a comprehensive list of stores and products that you can shop for all in once place!
            </Typography>
            <Typography variant="h6" sx={{
                marginTop: theme.spacing(20),
                textAlign: "center"
            }}>
                Select a category to begin browsing:
            </Typography>
            <Grid container spacing={5} justifyContent="center">
                <Grid item xs={12} sm={12} md={4}>
                    <Card variant="outlined" onClick={() => navigate("/products")}>
                        <CardHeader title={"Products"} sx={{
                            textAlign: "center"
                        }} />
                        <Divider />
                        <CardContent>
                            <Typography>
                                List of all available products in database
                            </Typography>
                        </CardContent>
                    </Card>
                </Grid>
                <Grid item xs={12} sm={12} md={4}>
                    <Card variant="outlined" onClick={() => navigate("/stores")}>
                        <CardHeader title={"Stores"} sx={{
                            textAlign: "center"
                        }} />
                        <Divider />
                        <CardContent>
                            <Typography>
                                List of all available stores in database
                            </Typography>
                        </CardContent>
                    </Card>
                </Grid>
            </Grid>
        </React.Fragment>
    )
}

export default LandingPage;