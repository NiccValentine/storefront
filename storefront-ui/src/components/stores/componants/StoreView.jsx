import React from 'react';
import { Card, CardContent, CardHeader, Divider, Grid, IconButton, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Toolbar, Typography, useTheme, } from '@mui/material';
import { Edit, KeyboardArrowLeft, KeyboardArrowRight } from '@mui/icons-material';
import { useNavigate, useParams } from 'react-router-dom';
import StoreService from '../../../shared/services/storeService';
import { Box } from '@mui/system';
import ProductService from '../../../shared/services/productService';

const storeService = new StoreService();
const productService = new ProductService();

const StoreView = () => {

    const navigate = useNavigate();
    const theme = useTheme();
    const { storeId } = useParams();

    const [store, setStore] = React.useState({})
    const [products, setProducts] = React.useState([])

    React.useEffect(() => {
        storeService.getSingle(storeId).then(response => {
            setStore(response.data)
        });

        productService.getProductsByStoreId(storeId).then(response => {
            setProducts(response.data)
        });
    }, []);

    return (
        <React.Fragment>
            <Toolbar>
                <IconButton onClick={() => navigate('/stores')} color="inherit">
                    <KeyboardArrowLeft fontSize='large' />
                </IconButton>
                <Typography variant='h3'>
                    {store.storeName}
                </Typography>
                <Box sx={{
                    flexGrow: 1
                }} />
                <IconButton onClick={() => navigate(`/stores/${storeId}/edit`)}>
                    <Edit fontSize='large' />
                </IconButton>
            </Toolbar>
            <Divider />
            <Grid container spacing={3} justifyContent="center">
                <Grid item xs={7} sm={6} md={5}>
                    <Card variant='outlined'>
                        <CardHeader title={store.storeName} />
                        <Divider />
                    </Card>
                </Grid>
                <Grid item xs={7} sm={6} md={9}>
                    <Card variant='outlined' sx={{
                        minHeight: "150px"
                    }}>
                        <CardContent>
                            <Typography variant='h6' align='center'>
                                Store Description
                            </Typography>
                            <Typography variant='subtitle1' align='center'>
                                {store.storeDescription}
                            </Typography>
                        </CardContent>
                    </Card>
                </Grid>
            </Grid>
            <Divider sx={{
                marginTop: theme.spacing(2)
            }} />
            <TableContainer component={Paper} sx={{
                marginTop: theme.spacing(2)
            }}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell>
                                <Typography variant='h6'>
                                    Products found in this store
                                </Typography>
                            </TableCell>
                            <TableCell />
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {products.length > '0' && products.map(product =>
                            <TableRow>
                                <TableCell>
                                    <Typography>
                                        {product.productName}
                                    </Typography>
                                </TableCell>
                                <TableCell padding='checkbox'>
                                    <IconButton onClick={() => navigate(`/products/${product.productId}`)}>
                                        <KeyboardArrowRight />
                                    </IconButton>
                                </TableCell>
                            </TableRow>)}
                    </TableBody>
                </Table>
            </TableContainer>
        </React.Fragment>
    )
}

export default StoreView;