import { Add, KeyboardArrowLeft } from "@mui/icons-material";
import { Divider, FormControl, FormGroup, Grid, IconButton, TextField, Toolbar, Typography, useTheme, Box } from "@mui/material";
import React from "react";
import { useNavigate } from "react-router-dom";
import ProductCard from "../../../shared/components/ProductCard";
import { v4 as uuid } from 'uuid';
import ProductService from "../../../shared/services/productService";

const productService = new ProductService();

const ProductSearch = () => {

    const theme = useTheme();
    const navigate = useNavigate();

    const [products, setProducts] = React.useState([]);

    const [searchTerm, setSearchTerm] = React.useState('');

    React.useEffect(() => {
        const timeOutId = setTimeout(() => {
            (() => {
                if (searchTerm !== '') {
                    productService.search(searchTerm)
                        .then(response => {
                            if (response.status = 200) {
                                setProducts(response.data)
                            }
                            else {
                                setProducts([])
                            }
                        })
                }
                else {
                    productService.get().then(response => {
                        setProducts(response.data);
                    })
                }
            })();
        }, 500);
        return () => {
            clearTimeout(timeOutId);
        }
    }, [searchTerm]);

    return (
        <React.Fragment>
            <Toolbar>
                <IconButton onClick={() => navigate("/")} color="inherit">
                    <KeyboardArrowLeft fontSize="large" />
                </IconButton>
                <Typography variant="h3">
                    Product Search
                </Typography>
                <Box sx={{
                    flexGrow: 1
                }} />
                <IconButton onClick={() => navigate(`/products/${uuid()}/edit`)} color="inherit">
                    <Add fontSize="large" />
                </IconButton>
            </Toolbar>
            <Divider />
            <FormGroup>
                <FormControl sx={{
                    alignSelf: 'center',
                    justifySelf: 'center',
                    width: '40%',
                    minWidth: '300px',
                    margin: theme.spacing(2)
                }}>
                    <TextField
                        label="Search"
                        value={searchTerm}
                        onChange={(event) => setSearchTerm(event.target.value)}
                    />
                </FormControl>
            </FormGroup>
            <Grid container spacing={3} sx={{
                margin: theme.spacing(1)
            }}>
                {products.length > '0' && products.map(product => <ProductCard product={product} url={`/products/${product.productId}`} />)}
            </Grid>
        </React.Fragment>
    )
}

export default ProductSearch;