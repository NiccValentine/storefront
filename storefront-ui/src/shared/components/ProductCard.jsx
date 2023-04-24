import { Card, CardContent, CardHeader, Divider, Typography } from '@mui/material';
import React from 'react';
import { useNavigate } from 'react-router-dom';

const ProductCard = ({ product, url }) => {

    const navigate = useNavigate();

    return (
        <Card
            variant='outlined'
            key={product.productId}
            onClick={() => navigate(url)}
            sx={{
                margin: "8px",
                width: "250px",
                height: "200px"
            }}>
            <CardHeader title={product.productName} />
            <Divider />
            <CardContent>
                <Typography>
                    {product.productDescription}
                </Typography>
            </CardContent>
        </Card>
    )
}

export default ProductCard;