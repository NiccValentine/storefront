import { Add, KeyboardArrowLeft, RemoveCircleOutline, Save } from '@mui/icons-material';
import { Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, Divider, FormControl, FormGroup, IconButton, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, TextField, Toolbar, Typography, useTheme } from '@mui/material';
import { Box } from '@mui/system';
import React from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import ConfirmationDialog from '../../../shared/components/ConfirmationDialog';
import useMessageProvider from '../../../shared/hooks/messageprovider/useMessageProvider';
import ProductService from '../../../shared/services/productService';
import StoreProductService from '../../../shared/services/storeProductService';
import StoreService from "../../../shared/services/storeService"

const storeService = new StoreService();
const productService = new ProductService();
const storeProductService = new StoreProductService();

const StoreEdit = () => {

    const { addMessage } = useMessageProvider();
    const { addError } = useMessageProvider();
    const theme = useTheme();
    const navigate = useNavigate();
    const { storeId } = useParams();
    const [store, setStore] = React.useState({
        storeId: null,
        storeName: "",
        storeDescription: ""
    })
    const [products, setProducts] = React.useState([])
    const [product, setProduct] = React.useState(null)
    const [unlinkedProducts, setUnlinkedProducts] = React.useState([])
    const [addDialogOpen, setAddDialogOpen] = React.useState(false)
    const [errors, setErrors] = React.useState([])
    const [confirmationOpen, setConfirmationOpen] = React.useState(false)


    React.useEffect(() => {
        storeService.getSingle(storeId).then(response => {
            if (response.status === 200) {
                setStore(response.data)
            }
        });

        productService.getProductsByStoreId(storeId).then(response => {
            setProducts(response.data)
        });
    }, []);

    const navBack = () => {
        if (store.storeId === null) {
            addError("Add store cancelled")
            navigate("/stores")
        }
        else {
            navigate(`/stores/${storeId}`)
        }
    }

    const handleChange = (property, newValue) => {
        setStore({ ...store, [property]: newValue })
    }

    const handleClose = () => {
        setConfirmationOpen(false)
        setProduct(null)
    }

    const handleProductDeleteClick = (product) => {
        setProduct(product)
        setConfirmationOpen(true)
    }

    const confirmDelete = () => {
        storeProductService.delete(storeId, product.productId)
            .then((response) => {
                if (response.status === 200) {
                    addMessage("Delete was successful")
                    productService.getProductsByStoreId(storeId).then(response => {
                        setProducts(response.data)
                    });
                }
            })
        handleClose()
    }

    const handleAddProductOpen = () => {
        setAddDialogOpen(true)

        productService.getProductsNotMatchingStoreId(storeId).then(response => {
            setUnlinkedProducts(response.data)
        })
    }

    const handleAddProductClose = () => {
        setAddDialogOpen(false)
    }

    const addProductToStore = (productId) => {
        const storeProduct = {
            storeId: storeId,
            productId: productId,
            store: null,
            product: null
        }

        storeProductService.post(storeProduct)
            .then((response) => {
                if (response.status === 201) {
                    productService.getProductsByStoreId(store.storeId).then(response => {
                        setProducts(response.data)
                    });
                }
            });

        handleAddProductClose();
    }

    const saveStore = () => {
        if (store.storeId === null || store.storeId === undefined) {
            store.storeId = storeId
            storeService.post(store)
                .then((response) => {
                    if (response.status === 201) {
                        addMessage("Save successful!")
                        navigate(`/stores/${storeId}`)
                    }
                }).catch((error) => {
                    if (error.response.status === 400) {
                        addError("There were validation errors")
                        store.storeId = null
                        setErrors(error.response.data.messages)
                    }
                });
        }
        else {
            storeService.put(store, storeId)
                .then((response) => {
                    if (response.status === 200) {
                        addMessage("Save successful!")
                        navigate(`/stores/${storeId}`)
                    }
                }).catch((error) => {
                    if (error.response.status === 400) {
                        addError("There were validation erorrs")
                        setErrors(error.response.data.messages)
                    }
                });
        }
    }

    return (
        <React.Fragment>
            <Toolbar>
                <IconButton onClick={navBack} color="inherit">
                    <KeyboardArrowLeft fontSize='large' />
                </IconButton>
                <Typography variant='h3'>
                    Edit Store
                </Typography>
                <Box sx={{
                    flexGrow: 1
                }} />
                <IconButton onClick={saveStore} color={"info"}>
                    <Save fontSize='large' />
                </IconButton>
            </Toolbar>
            <Divider />
            <FormGroup>
                <FormControl sx={{
                    minWidth: '200px',
                    padding: theme.spacing(1)
                }}>
                    <TextField
                        label="Store Name"
                        value={store.storeName}
                        onChange={event => handleChange("storeName", event.target.value)}
                        error={errors && errors.find(message => message.fieldName === 'StoreName') !== undefined}
                        helperText={errors && errors.find(message => message.fieldName === 'StoreName')?.messageText}
                    />
                </FormControl>
                <FormControl sx={{
                    minWidth: '200px',
                    padding: theme.spacing(1)
                }}>
                    <TextField
                        label="Store Description"
                        value={store.storeDescription}
                        onChange={event => handleChange("storeDescription", event.target.value)}
                        multiline
                        minRows={3}
                        error={errors && errors.find(message => message.fieldName === 'StoreDescription') !== undefined}
                        helperText={errors && errors.find(message => message.fieldName === 'StoreDescription')?.messageText}
                    />
                </FormControl>
            </FormGroup>
            <TableContainer component={Paper} sx={{
                marginTop: theme.spacing(2)
            }}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell>
                                <Typography variant='h6'>
                                    Product found at this store
                                </Typography>
                            </TableCell>
                            <TableCell padding='checkbox'>
                                <IconButton onClick={() => handleAddProductOpen()}>
                                    <Add fontSize='large' />
                                </IconButton>
                                <Dialog
                                    open={addDialogOpen}
                                    onClose={handleAddProductClose}>
                                    <DialogTitle>
                                        Add Product
                                    </DialogTitle>
                                    <DialogContent>
                                        <DialogContentText>
                                            Choose a product to add to this store
                                        </DialogContentText>
                                        <Table>
                                            <TableHead>
                                                <TableRow>
                                                    <TableCell>
                                                        <Typography variant='h6'>
                                                            Products available
                                                        </Typography>
                                                    </TableCell>
                                                </TableRow>
                                            </TableHead>
                                            <TableBody>
                                                {unlinkedProducts.length > '0' && unlinkedProducts.map(product =>
                                                    <TableRow hover onClick={() => addProductToStore(product.productId)}>
                                                        <TableCell>
                                                            <Typography>
                                                                {product.productName}
                                                            </Typography>
                                                        </TableCell>
                                                        <TableCell>

                                                        </TableCell>
                                                    </TableRow>
                                                )}
                                            </TableBody>
                                        </Table>
                                    </DialogContent>
                                    <DialogActions>
                                        <Button onClick={handleAddProductClose}>Cancel</Button>
                                    </DialogActions>
                                </Dialog>
                            </TableCell>
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
                                    <IconButton onClick={() => handleProductDeleteClick(product)}>
                                        <RemoveCircleOutline color='error' fontSize='large' />
                                    </IconButton>
                                </TableCell>
                            </TableRow>)}
                    </TableBody>
                </Table>
            </TableContainer>
            <ConfirmationDialog open={confirmationOpen} handleClose={handleClose} dialogTitle={"Product Delete"} dialogMessage={"Pressing accept will delete this product."} confirmationAction={confirmDelete} />
        </React.Fragment>
    )
}

export default StoreEdit;