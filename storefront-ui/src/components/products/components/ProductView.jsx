import { Edit, KeyboardArrowLeft, KeyboardArrowRight } from "@mui/icons-material";
import { Card, CardContent, CardHeader, Divider, Grid, IconButton, Toolbar, Typography, useTheme, Box, TableContainer, Table, TableHead, TableRow, TableCell, TableBody, Paper } from "@mui/material";
import React from "react";
import { useNavigate, useParams } from "react-router-dom";
import ProductService from "../../../shared/services/productService";
import StoreService from "../../../shared/services/storeService";

const productService = new ProductService();
const storeService = new StoreService();

const ProductView = () => {

    const theme = useTheme();
    const navigate = useNavigate();
    const { productId } = useParams();

    const [product, setProduct] = React.useState({})
    const [stores, setStores] = React.useState([])

    React.useEffect(() => {
        productService.getSingle(productId).then(response => {
            setProduct(response.data)
        });

        storeService.getStoresByProductId(productId).then(response => {
            setStores(response.data)
        });
    }, []);

    return (
        <React.Fragment>
            <Toolbar>
                <IconButton onClick={() => navigate("/products")} color="inherit">
                    <KeyboardArrowLeft fontSize="large" />
                </IconButton>
                <Typography variant="h3">
                    {product.productName}
                </Typography>
                <Box sx={{
                    flexGrow: 1
                }} />
                <IconButton onClick={() => navigate(`/products/${productId}/edit`)}>
                    <Edit fontSize="large" />
                </IconButton>
            </Toolbar>
            <Divider />
            <Grid container spacing={3} justifyContent="center">
                <Grid item xs={7} sm={6} md={5}>
                    <Card variant="outlined">
                        <CardHeader title={product.productName} />
                        <Divider />
                    </Card>
                </Grid>
                <Grid item xs={7} sm={6} md={9}>
                    <Card variant="outlined" sx={{
                        minHeight: "150px"
                    }}>
                        <CardContent>
                            <Typography variant="h6" align="center">
                                Product Description
                            </Typography>
                            <Typography variant="subtitle1" align="center">
                                {product.productDescription}
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
                                <Typography variant="h6">
                                    Stores that have this product
                                </Typography>
                            </TableCell>
                            <TableCell />
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {stores.length > '0' && stores.map(store =>
                            <TableRow>
                                <TableCell>
                                    <Typography>
                                        {store.storeName}
                                    </Typography>
                                </TableCell>
                                <TableCell padding="checkbox">
                                    <IconButton onClick={() => navigate(`/stores/${store.storeId}`)}>
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

export default ProductView;