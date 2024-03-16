import * as React from "react";
import Backdrop from "@mui/material/Backdrop";
import { useSelector, useDispatch } from "react-redux";
import { openBlockUI, closeBlockUI } from "store/counterSlice";
import "../BlockUI/blockUI.css";
const BlockUI = () => {
  const isOpent = useSelector((state: any) => state.counterSlice?.open);
  return (
    <div>
      <Backdrop
        sx={{
          color: "#fff",
          zIndex: (theme) => theme.zIndex.drawer + 1,
          cursor: isOpent ? "progress" : "",
        }}
        open={false}
      >
        <div className="block-ui-container">
          <div className="bookshelf_wrapper">
            <ul className="books_list">
              <li className="book_item_1 first"></li>
              <li className="book_item_2 second"></li>
              <li className="book_item_3 third"></li>
              <li className="book_item_1 fourth"></li>
              <li className="book_item_2 fifth"></li>
              <li className="book_item_3 sixth"></li>
            </ul>
            <div className="shelf"></div>
            <div className="textBlock">NOW LOADING</div>
          </div>
          {/* Loading 2 */}
          {/* <div className="Main-Box">
            <div className="container">
              <div className="📦"></div>
              <div className="📦"></div>
              <div className="📦"></div>
              <div className="📦"></div>
              <div className="📦"></div>
            </div>
            <div className="loading-process">Loading in progress...</div>
          </div> */}
        </div>
      </Backdrop>
    </div>
  );
};

export const FnBlock_UI = () => {
  const Dispatch = useDispatch();
  const BlockUI = () => Dispatch(openBlockUI());
  const UnBlockUI = () => Dispatch(closeBlockUI());
  return { BlockUI, UnBlockUI };
};

export default BlockUI;
