import { Card, CardContent, CardHeader, Divider, Typography } from '@mui/material';
import React from 'react';
import { useNavigate } from 'react-router-dom';

const StoreCard = ({ store, url }) => {

    const navigate = useNavigate();

    return (
        <Card
            variant='outlined'
            key={store.storeId}
            onClick={() => navigate(url)}
            sx={{
                margin: "8px",
                width: "250px",
                height: "200px"
            }}>
            <CardHeader title={store.storeName} />
            <Divider />
            <CardContent>
                <Typography>
                    {store.storeDescription}
                </Typography>
            </CardContent>
        </Card>
    )
}

export default StoreCard;